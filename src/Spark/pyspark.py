df = spark.read.option("header", "true").csv("/home/admmatt/data.csv")

df.createOrReplaceTempView("data")

sqlDf = spark.sql("SELECT * FROM data")

sqlDF.show()

sqlDF = spark.sql("select DEST_COUNTRY_NAME, SUM(count) from flights GROUP BY(DEST_COUNTRY_NAME)")

