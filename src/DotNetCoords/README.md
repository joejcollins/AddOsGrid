# .NET Coordinates

This C# library can be used to convert from one mapping coordinate system to another, for instance converting latitude/longitude to UK OS coordinates. 
It is based on the JCoord class library and is therefore licensed under the GPL. It also should have all the features available in that library.

And here's a simple example of usage. Other than changing the datum, the classes are immutable i.e Their state does not change after instantiation. 
They have methods to convert to other coordinate systems which return an instance of the required type.

```cs
using System;
using DotNetCoords;

namespace TestDotNetCoords
{
  class Program
  {
    static void Main(string[] args)
    {
      // create an OS grid reference object
      OSRef osRef = new OSRef(535598, 182120);
      Console.WriteLine("OS reference is " + osRef.ToString());
      Console.WriteLine("Grid reference is " + osRef.ToSixFigureString());

      LatLng latLng = osRef.ToLatLng();
      Console.WriteLine("Lat/long using OSGB36 datum is " + latLng.ToString());

      latLng.ToWGS84();
      Console.WriteLine("Lat/long using WGS84 datum is " + latLng.ToString());

      MGRSRef mgrsRef = latLng.ToMGRSRef();
      Console.WriteLine("MGRS reference is " + mgrsRef.ToString());

      UTMRef utmRef = mgrsRef.ToUTMRef();
      Console.WriteLine("UTM reference is " + utmRef.ToString());

      Console.ReadLine();
    }
  }
}
```

Copied whole sale from <https://www.doogal.co.uk/dotnetcoords.php>.