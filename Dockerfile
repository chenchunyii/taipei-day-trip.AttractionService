# 基礎鏡像，用於建置應用程式
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app
EXPOSE 5197

# 複製 .csproj 檔案並還原 NuGet 套件
COPY *.csproj ./
RUN dotnet restore

# 複製所有原始碼
COPY . ./

# 建置應用程式
RUN dotnet publish -c Release -o out

# 最終鏡像，用於運行應用程式
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
EXPOSE 5197

# 從建置階段複製發布的輸出
COPY --from=build /app/out ./

# 設定啟動命令
ENTRYPOINT ["dotnet", "taipei-day-trip-dotnet.dll"]