# Use the official .NET SDK image to build and run tests
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["QualityManager.Test/QualityManager.Test.csproj", "QualityManager.Test/"]
COPY ["Shared/Shared.csproj", "Shared/"]
COPY ["QualityManager/QualityManager.csproj", "QualityManager/"]


RUN dotnet restore "QualityManager.Test/QualityManager.Test.csproj"

COPY QualityManager.Test/ QualityManager.Test/
COPY Shared/ Shared/
COPY QualityManager/ QualityManager/
