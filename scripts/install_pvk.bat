C:\mysql\bin\mysql --host=localhost -u root <"root_pwd.sql"

C:\mysql\bin\mysql -u root --password=masterkey <"create_pvk.sql"

C:\mysql\bin\mysql -u root --password=masterkey -f <"create_storage_pvk.sql"

C:\mysql\bin\mysql -u root --password=masterkey pvk -f <"create_mysql_pvk_users.sql"

C:\mysql\bin\mysql -u root --password=masterkey pvk -f <"create_data_pvk.sql"
