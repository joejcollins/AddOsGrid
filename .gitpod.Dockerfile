FROM gitpod/workspace-dotnet

USER gitpod

# Install Starship
RUN curl -fsSL https://starship.rs/install.sh | bash -s -- --yes
