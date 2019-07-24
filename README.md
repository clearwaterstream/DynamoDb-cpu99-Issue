# DynamoDb-cpu99-Issue

### Update -- the issue can no longer be reproduced when using AWSSDK.DynamoDBv2 v3.3.104.8

An example code that shows how GetNextSetAsync() can overwhelm a box running IIS / ASP.NET Core

* The app is configured to run in `us-east-1`. If you need to chang the region, see [appsettings.json](https://github.com/clearwaterstream/DynamoDb-cpu99-Issue/blob/master/src/DynamoDBCPU99/appsettings.json) and [NLog.config](https://github.com/clearwaterstream/DynamoDb-cpu99-Issue/blob/master/src/DynamoDBCPU99/NLog.config). If running on EC2, the region is inferred from instance metadata.

* DynamoDB table `ZipCode` needs to be created with `Id` as the primary key and GSI on `Code`. EC2 needs to have ability to read / write into the table.

* If running localy, `dev` AWS profile is used. If you need to change the name, change `aws_profile` in [appsettings.Development.json](https://github.com/clearwaterstream/DynamoDb-cpu99-Issue/blob/master/src/DynamoDBCPU99/appsettings.Development.json)

To seed sample data into `ZipCode` table, run `\home\seed` GET method  

To execute concurrenct search operations on your local machine, do `ab.exe -n 1000 -c 15 http://localhost:port_num/home/search`. Apache for Windows can be downloaded [here.](https://www.apachelounge.com/download/)
