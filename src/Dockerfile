FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src

# set up node
ENV NODE_VERSION 10.5.0
ENV NODE_DOWNLOAD_SHA 5d77d2c68c06404028f063dca0947315570ff5e52e46f67f93ef9f6cdcb1b4a8
RUN curl -SL "https://nodejs.org/dist/v${NODE_VERSION}/node-v${NODE_VERSION}-linux-x64.tar.gz" --output nodejs.tar.gz \
&& echo "$NODE_DOWNLOAD_SHA nodejs.tar.gz" | sha256sum -c - \
&& tar -xzf "nodejs.tar.gz" -C /usr/local --strip-components=1 \
&& rm nodejs.tar.gz \
&& ln -s /usr/local/bin/node /usr/local/bin/nodejs

RUN npm install
RUN npm run release
RUN dotnet build "GameATron4000.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "GameATron4000.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "GameATron4000.dll"]
