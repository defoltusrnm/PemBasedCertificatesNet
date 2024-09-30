# PemBasedCertificatesNet

## Install OpenSSL (Windows winget)
* ### Run command (У меня он был каким-то образом)
* ### `winget install openssl`

## Create self-signed PEM certificate and place to .csproj
```openssl req -x509 -newkey rsa:4096 -keyout key.pem -out cert.pem -sha256 -days 3650 -nodes -subj "/C=XX/ST=StateName/L=CityName/O=CompanyName/OU=CompanySectionName/CN=CommonNameOrHostname"```

## Run in src/PemBasedCertificates.Net `docker-compose up -d`

## ??? profit