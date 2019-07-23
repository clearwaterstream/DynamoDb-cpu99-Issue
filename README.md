# DynamoDb-cpu99-Issue

An example code that shows how GetNextSetAsync() can overwhelm a box running IIS / ASP.NET Core

* The app is configured to run in `us-east-1`. If you need to chang the region, see [appsettings.json](https://github.com/clearwaterstream/DynamoDb-cpu99-Issue/blob/master/src/DynamoDBCPU99/appsettings.json) and [NLog.config]()https://github.com/clearwaterstream/DynamoDb-cpu99-Issue/blob/master/src/DynamoDBCPU99/NLog.config. If running on EC2, region is inferred from instance metadata.
* DynamoDB table `ZipCode` needs to be created. EC2 needs to have ability to read / write into the table.
* If running localy, `dev` AWS profile is used. If you need to change the name, change `aws_profile` in [appsettings.Development.json](https://github.com/clearwaterstream/DynamoDb-cpu99-Issue/blob/master/src/DynamoDBCPU99/appsettings.Development.json)

To seed sample data into `ZipCode` table, run `\home\seed` GET method  

To execute concurrenct search operations, do `abs.exe \home\search`
