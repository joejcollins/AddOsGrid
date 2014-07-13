// From http://denwilliams.net/2013/07/17/gps-metadata/

using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Text;
using System.Linq;

namespace ConsoleApp
{
    public static class ImageExtensions
    {
        /// <summary>
        /// Gets the date the image was taken.
        /// </summary>
        /// <param name="image">The image.</param>
        /// <returns></returns>
        public static DateTime? GetDateTaken(this Image image)
        {
            try
            {
                //DateTimeOriginal
                PropertyItem propItem = image.GetPropertyItem(0x9003);
                // See also - DateTimeDigitized / CreateDate 0x9004
                // .. TimeZoneOffset 0x882a & ModifyDate (DateTime) 0x0132

                //Convert date taken metadata to a DateTime object
                string sdate = Encoding.UTF8.GetString(propItem.Value).Replace("\0", String.Empty).Trim();
                string secondhalf = sdate.Substring(sdate.IndexOf(" "), (sdate.Length - sdate.IndexOf(" ")));
                string firsthalf = sdate.Substring(0, 10);
                firsthalf = firsthalf.Replace(":", "-");
                sdate = firsthalf + secondhalf;
                return DateTime.Parse(sdate);
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Gets the datetime stamp from the GPS metadata (more likely to be accurate than camera timestamp).
        /// </summary>
        /// <param name="image">The image.</param>
        /// <returns></returns>
        public static DateTime? GetGpsDateTimeStamp(this Image image)
        {
            try
            {
                //GPSTimeStamp
                PropertyItem propTime = image.GetPropertyItem(0x0007);
                uint hours = GetExifSubValue(propTime, 0);
                uint mins = GetExifSubValue(propTime, 1);
                uint secs = GetExifSubValue(propTime, 2);
                string stime = string.Format("{0:00}:{1:00}:{2:00}", hours, mins, secs);

                //GPSDateStamp
                PropertyItem propDate = image.GetPropertyItem(0x001d);

                //Convert date taken metadata to a DateTime object
                string sdate = Encoding.UTF8.GetString(propDate.Value).Replace("\0", String.Empty).Trim();
                sdate = sdate.Replace(":", "-");
                return DateTime.Parse(sdate + " " + stime);
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Gets the GPS info for the image.
        /// </summary>
        /// <param name="image">The image.</param>
        /// <returns></returns>
        public static GpsMetaData GetGpsInfo(this Image image)
        {
            float? lat = GetLatitude(image);
            float? lon = GetLongitude(image);
            var dTs = GetGpsDateTimeStamp(image);
            var alt = GetAltitude(image);
            var dTaken = GetDateTaken(image);
            var result = new GpsMetaData();
            if (lat.HasValue) result.Latitude = lat.Value;
            if (lon.HasValue) result.Longitude = lon.Value;
            if (alt.HasValue) result.Altitude = alt.Value;
            if (dTs.HasValue) result.Timestamp = dTs.Value;
            else if (dTaken.HasValue) result.Timestamp = dTaken.Value;

            return result;
        }

        /// <summary>
        /// Gets the latitude component from the GPS metadata.
        /// </summary>
        /// <param name="image">The image.</param>
        /// <returns></returns>
        public static float? GetLatitude(this Image image)
        {
            try
            {
                //PropertyTagGpsLatitudeRef - 'N' or 'S'
                PropertyItem propItemRef = image.GetPropertyItem(1);
                //PropertyTagGpsLatitude
                PropertyItem propItemLat = image.GetPropertyItem(2);
                return ExifGpsToFloat(propItemRef, propItemLat);
            }
            catch (ArgumentException)
            {
                return null;
            }
        }

        /// <summary>
        /// Gets the longitude component from the GPS metadata.
        /// </summary>
        /// <param name="image">The image.</param>
        /// <returns></returns>
        public static float? GetLongitude(this Image image)
        {
            try
            {
                //PropertyTagGpsLongitudeRef - 'E' or 'W'
                PropertyItem propItemRef = image.GetPropertyItem(3);
                //PropertyTagGpsLongitude
                PropertyItem propItemLong = image.GetPropertyItem(4);
                return ExifGpsToFloat(propItemRef, propItemLong);
            }
            catch (ArgumentException)
            {
                return null;
            }
        }

        /// <summary>
        /// Gets the altitude component from the GPS metadata.
        /// </summary>
        /// <param name="image">The image.</param>
        /// <returns></returns>
        public static float? GetAltitude(this Image image)
        {
            try
            {
                //GPSAltitudeRef - 0 (above sea level) or 1 (below sea level)
                PropertyItem propItemRef = image.GetPropertyItem(0x0005);
                //GPSAltitude
                PropertyItem propItemLong = image.GetPropertyItem(0x0006);
                float value = GetExifSubValue(propItemLong, 0);
                if (propItemRef.Value[0] == 1)
                    value = 0 - value;
                return value;
            }
            catch (ArgumentException)
            {
                return null;
            }
        }

        private static float ExifGpsToFloat(PropertyItem propItemRef, PropertyItem propItem)
        {
            uint degrees = GetExifSubValue(propItem, 0);
            uint minutes = GetExifSubValue(propItem, 1);
            uint seconds = GetExifSubValue(propItem, 2);

            float coorditate = degrees + (minutes / 60f) + (seconds / 3600f);
            string gpsRef = System.Text.Encoding.ASCII.GetString(new byte[1] { propItemRef.Value[0] }); //N, S, E, or W
            if (gpsRef == "S" || gpsRef == "W")
                coorditate = 0 - coorditate;
            return coorditate;
        }

        private static uint GetExifSubValue(PropertyItem property, int index)
        {
            int baseIndex = index * 8;
            uint numerator = BitConverter.ToUInt32(property.Value, baseIndex);
            uint denominator = BitConverter.ToUInt32(property.Value, baseIndex + 4);
            return numerator / denominator;
        }

        /// <summary>
        /// Gets the description of the image from the ImageDescription EXIF data.
        /// </summary>
        /// <param name="image">The image.</param>
        /// <returns>A string value, or null if not found</returns>
        public static string GetDescription(this Image image)
        {
            try
            {
                //ImageDescription
                const int exifId = 0x010e;
                if (!image.PropertyIdList.Contains(exifId)) return null;
                PropertyItem propItem = image.GetPropertyItem(exifId);
                return Encoding.UTF8.GetString(propItem.Value).Replace("\0", String.Empty).Trim();
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Gets a new description for the image to the ImageDescription EXIF value.
        /// </summary>
        /// <param name="image">The image.</param>
        /// <param name="description">The description to set.</param>
        /// <returns>
        /// true if succeeded, false otherwise
        /// </returns>
        public static bool SetDescription(this Image image, string description)
        {
            try
            {
                //ImageDescription
                const int exifId = 0x010e;
                // note: ideally we would do image.GetPropertyItem(0x010e),
                // but if there isn't a description already that would fail.
                // This is actually the recommendation from Microsoft -
                // to get any other property, modify it, then set it back!
                PropertyItem propItem = image.PropertyItems[0];
                SetExifProperty(ref propItem, exifId, description);
                image.SetPropertyItem(propItem);
                return true;
            }
            catch
            {
                return false;
            }
        }

        //My SetProperty code... (for ASCII property items only!)
        //Exif 2.2 requires that ASCII property items terminate with a null (0x00).
        private static void SetExifProperty(ref PropertyItem prop, int id, string text)
        {
            var encodedString = Encoding.UTF8.GetBytes(text);

            int iLen = encodedString.Length + 1;
            byte[] bTxt = new byte[iLen];
            for (int i = 0; i < iLen - 1; i++)
                bTxt[i] = encodedString[i];
            bTxt[iLen - 1] = 0x00; // null terminated string
            prop.Id = id;
            prop.Type = 2;
            prop.Value = bTxt;
            prop.Len = iLen;
        }

    }

    /// <summary>
    /// GPS EXIF Metadata stored with geotagged images
    /// </summary>
    public class GpsMetaData
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double Altitude { get; set; }
        public DateTime Timestamp { get; set; }
    }
}