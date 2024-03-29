use pvk;

CREATE TABLE IF NOT EXISTS `pvk`.`PVK_ATTRIBUTES`(  
  `ID` INT NOT NULL COMMENT 'Идентификатор' AUTO_INCREMENT,
  `NAME` VARCHAR(200) NOT NULL COMMENT 'Имя справочника',
  `ABBR` VARCHAR(200) NOT NULL COMMENT 'Аббревиатура',
  `TYPE` INT COMMENT 'Тип',
  PRIMARY KEY (`ID`)
) ;

CREATE TABLE IF NOT EXISTS `PVK_ATTR_VALUES` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `ATTR_ID` int(11) DEFAULT NULL,
  `VALUE` varchar(255)  DEFAULT NULL,
  `DESCRIPTION` varchar(255)  DEFAULT NULL,
  `NAME` varchar(200)  DEFAULT NULL,
  `ABBR` VARCHAR(200) NOT NULL COMMENT 'Аббревиатура',
  PRIMARY KEY (`ID`)
) ;

CREATE TABLE IF NOT EXISTS `pvk`.`PVK_MODEL_VEHICLE`(  
  `ID` INT NOT NULL AUTO_INCREMENT,
  `MARK` VARCHAR(255),
  `MODEL` VARCHAR(255),
  `TYPE` INT,
  `COUNT_AXIS` INT,
  `AXIS_LENGTH_1` DOUBLE,
  `AXIS_LENGTH_2` DOUBLE,
  `AXIS_LENGTH_3` DOUBLE,
  `AXIS_LENGTH_4` DOUBLE,
  `AXIS_LENGTH_5` DOUBLE,
  `AXIS_LENGTH_6` DOUBLE,
  `AXIS_LENGTH_7` DOUBLE,
  `AXIS_LENGTH_8` DOUBLE,
  `AXIS_LENGTH_9` DOUBLE,
  `AXIS_LENGTH_10` DOUBLE,
  `AXIS_LENGTH_11` DOUBLE,
  `AXIS_LENGTH_12` DOUBLE,
  `LINK_LENGTH` DOUBLE,
  PRIMARY KEY (`ID`)
);

CREATE TABLE IF NOT EXISTS `pvk`.`PVK`(  
  `ID` INT NOT NULL AUTO_INCREMENT,
  `ADDRESS` VARCHAR(255),
  `NOMER` VARCHAR(200),
  `ORGANIZATIONS` VARCHAR(255),
  `SPECIALIST_PVK` VARCHAR(255),
  `INSPECTOR_PVK` VARCHAR(255),
  `INSPECTOR_GIBDD` VARCHAR(255),
  `CAMERA_URL` VARCHAR(255),
  `CAMERA_LOGIN` VARCHAR(255),
  `CAMERA_PASS` VARCHAR(255),
  `NUMBER_COM_PORT` INT,
  `SCALES_ID` INT,
  `SPRING_LIMIT` INT,
  PRIMARY KEY (`ID`)
) ;

CREATE TABLE IF NOT EXISTS `pvk`.`PVK_SCALES`(  
  `ID` INT NOT NULL AUTO_INCREMENT,
  `NAME` VARCHAR(255),
  `NOMER` VARCHAR(255),
  `DATE_CKEKING` DATETIME,
  PRIMARY KEY (`ID`)
) ;

CREATE TABLE IF NOT EXISTS `pvk`.`PVK_WEIGHING`(  
  `ID` INT NOT NULL AUTO_INCREMENT,
  `PVK_ID` INT,
  `DATE_WEIGHING` DATE,
  `OWNER_VEHICLE` VARCHAR(255),
  `MODEL_VEHICLE_ID` INT,
  `VEHICLE_REG_NUMBER` VARCHAR(255),
  `VEHICLE_PODVESKA` VARCHAR(255),
  `MODEL_TRAILER_ID` INT,
  `TRAILER_REG_NUMBER` VARCHAR(255),
  `TRAILER_PODVESKA` VARCHAR(255),
  `ROUTE_NAME` VARCHAR(255),
  `COUNT_AXIS_VEHICLE` INT,
  `COUNT_AXIS_TRAILER` INT,
  `PHOTO` LONGBLOB NULL,
  `DISTANCE_AXIS_1` DOUBLE,
  `DISTANCE_AXIS_2` DOUBLE,
  `DISTANCE_AXIS_3` DOUBLE,
  `DISTANCE_AXIS_4` DOUBLE,
  `DISTANCE_AXIS_5` DOUBLE,
  `DISTANCE_AXIS_6` DOUBLE,
  `DISTANCE_AXIS_7` DOUBLE,
  `DISTANCE_AXIS_8` DOUBLE,
  `DISTANCE_AXIS_9` DOUBLE,
  `DISTANCE_AXIS_10` DOUBLE,
  `DISTANCE_AXIS_11` DOUBLE,
  `DISTANCE_AXIS_12` DOUBLE,
  `DISTANCE_AXIS_13` DOUBLE,
  `DISTANCE_AXIS_14` DOUBLE,
  `DISTANCE_AXIS_15` DOUBLE,
  `DISTANCE_AXIS_16` DOUBLE,
  `DISTANCE_AXIS_17` DOUBLE,
  `DISTANCE_AXIS_18` DOUBLE,
  `DISTANCE_AXIS_19` DOUBLE,
  `DISTANCE_AXIS_20` DOUBLE,
  `DISTANCE_AXIS_21` DOUBLE,
  `DISTANCE_AXIS_22` DOUBLE,
  `DISTANCE_AXIS_23` DOUBLE,
  `DISTANCE_AXIS_24` DOUBLE,
  `LOAD_AXIS_1` DOUBLE,
  `LOAD_AXIS_2` DOUBLE,
  `LOAD_AXIS_3` DOUBLE,
  `LOAD_AXIS_4` DOUBLE,
  `LOAD_AXIS_5` DOUBLE,
  `LOAD_AXIS_6` DOUBLE,
  `LOAD_AXIS_7` DOUBLE,
  `LOAD_AXIS_8` DOUBLE,
  `LOAD_AXIS_9` DOUBLE,
  `LOAD_AXIS_10` DOUBLE,
  `LOAD_AXIS_11` DOUBLE,
  `LOAD_AXIS_12` DOUBLE,
  `LOAD_AXIS_13` DOUBLE,
  `LOAD_AXIS_14` DOUBLE,
  `LOAD_AXIS_15` DOUBLE,
  `LOAD_AXIS_16` DOUBLE,
  `LOAD_AXIS_17` DOUBLE,
  `LOAD_AXIS_18` DOUBLE,
  `LOAD_AXIS_19` DOUBLE,
  `LOAD_AXIS_20` DOUBLE,
  `LOAD_AXIS_21` DOUBLE,
  `LOAD_AXIS_22` DOUBLE,
  `LOAD_AXIS_23` DOUBLE,
  `LOAD_AXIS_24` DOUBLE,
  `FACT_LOAD_AXIS_1` DOUBLE,
  `FACT_LOAD_AXIS_2` DOUBLE,
  `FACT_LOAD_AXIS_3` DOUBLE,
  `FACT_LOAD_AXIS_4` DOUBLE,
  `FACT_LOAD_AXIS_5` DOUBLE,
  `FACT_LOAD_AXIS_6` DOUBLE,
  `FACT_LOAD_AXIS_7` DOUBLE,
  `FACT_LOAD_AXIS_8` DOUBLE,
  `FACT_LOAD_AXIS_9` DOUBLE,
  `FACT_LOAD_AXIS_10` DOUBLE,
  `FACT_LOAD_AXIS_11` DOUBLE,
  `FACT_LOAD_AXIS_12` DOUBLE,
  `FACT_LOAD_AXIS_13` DOUBLE,
  `FACT_LOAD_AXIS_14` DOUBLE,
  `FACT_LOAD_AXIS_15` DOUBLE,
  `FACT_LOAD_AXIS_16` DOUBLE,
  `FACT_LOAD_AXIS_17` DOUBLE,
  `FACT_LOAD_AXIS_18` DOUBLE,
  `FACT_LOAD_AXIS_19` DOUBLE,
  `FACT_LOAD_AXIS_20` DOUBLE,
  `FACT_LOAD_AXIS_21` DOUBLE,
  `FACT_LOAD_AXIS_22` DOUBLE,
  `FACT_LOAD_AXIS_23` DOUBLE,
  `FACT_LOAD_AXIS_24` DOUBLE,
  `ACT_NOMER` INT,
  `ACT_NARUSHENIE_ID` INT,
  `ACT_CARGO_TYPE_ID` INT,
  `ACT_ROUTE_TYPE_ID` INT,
  `ACT_ROUTE_TYPE` VARCHAR(255),
  `ACT_EXPLANATION_DRIVER` VARCHAR(255),
  `ACT_DRIVER` VARCHAR(255),
  `ACT_ASSECCORY` VARCHAR(255) COMMENT 'Принадлежность',
  `ACT_PROTOKOL_NUMBER` VARCHAR(255),
  `ACT_PROTOCOL_DATE` DATE ,
  `ACT_SUBSCRIBE` VARCHAR(255) COMMENT 'Кем подписан',
  `ACT_DAMADGE` DOUBLE COMMENT 'Ущерб',
  `ACT_DETAILS_PAY_ID` VARCHAR(255) COMMENT 'Реквизиты',
  `ACT_NOTE` VARCHAR(255) COMMENT 'Примечание',
  `CARGO_TYPE_ID` VARCHAR(255) COMMENT 'Тип грузв',
  PRIMARY KEY (`ID`)
) ;

CREATE TABLE IF NOT EXISTS `pvk`.`PVK_PERMISSION_AXIS_LOAD`(  
  `ID` INT NOT NULL AUTO_INCREMENT,
  `ID_ADD` INT,
  `DISTANCE_MIN` DOUBLE,
  `DISTANCE_MAX` DOUBLE,
  `PERMISSION_VALUE_1` DOUBLE,
  `PERMISSION_VALUE_2` DOUBLE,
  `PERMISSION_VALUE_3` DOUBLE,
  `COUNT_AXIS_MIN` int DEFAULT NULL,
  `COUNT_AXIS_MAX` int DEFAULT NULL,
  PRIMARY KEY (`ID`)
) ;

CREATE TABLE IF NOT EXISTS `pvk`.`PVK_DAMAGE_TS_VALUE`(  
  `ID` INT NOT NULL AUTO_INCREMENT,
  `ID_RECALC` INT,
  `OVER_MIN` DOUBLE,
  `OVER_MAX` DOUBLE,
  `TYPE` VARCHAR(255),
  `FACTOR` VARCHAR(255),
  `FORMULA` VARCHAR(255),
  PRIMARY KEY (`ID`)
);

CREATE TABLE IF NOT EXISTS pvk.PVK_PERMISSION_WEIGH (
  ID                 INT NOT NULL,
  ID_RECALC        INT NOT NULL,
  COUNT_AXIS        INT,
  VALUE_AUTO DOUBLE,
  VALUE_TRAILER  DOUBLE,
  PRIMARY KEY (`ID`)
);

CREATE TABLE IF NOT EXISTS pvk.PVK_PERIOD_CALCULATE_DAMAGE_TS (
  ID           INT NOT NULL,
  BEG_DT		 DATE         NOT NULL,
  END_DT		 DATE,
  NOTE      VARCHAR(255),
  PRIMARY KEY (`ID`)
);


ALTER IGNORE  TABLE `pvk`.`pvk_weighing`  ADD COLUMN `PVK_ADDRESS` VARCHAR(255) NULL AFTER `ACT_NOTE`;

ALTER IGNORE  TABLE `pvk`.`pvk_weighing`  ADD COLUMN `PERMISSION` VARCHAR(255) NULL AFTER `ACT_NOTE`;

ALTER IGNORE  TABLE `pvk`.`pvk_weighing`   
ADD COLUMN `ACT_INSPECTOR_PVK` VARCHAR(255) NULL AFTER `PVK_ADDRESS`;


ALTER IGNORE  TABLE `pvk`.`pvk_weighing`   	
ADD COLUMN `ACT_SPECIALIST_PVK` VARCHAR(255) NULL AFTER `ACT_INSPECTOR_PVK`;

ALTER IGNORE  TABLE `pvk`.`pvk_weighing`   
ADD COLUMN `ACT_INSPECTOR_GIBDD` VARCHAR(255) NULL AFTER `ACT_SPECIALIST_PVK`;

ALTER IGNORE  TABLE `pvk`.`pvk_weighing`   
  CHANGE `DATE_WEIGHING` `DATE_WEIGHING` DATETIME NULL;

ALTER IGNORE  TABLE `pvk`.`pvk_weighing`   
 ADD COLUMN `ADDRESS_OWNER` VARCHAR(255) NULL AFTER `ACT_INSPECTOR_GIBDD`;
 
 ALTER IGNORE  TABLE `pvk`.`pvk_weighing`   
 ADD COLUMN `CARGO_TYPE_ID` VARCHAR(255) NULL;

ALTER IGNORE  TABLE `pvk`.`pvk_model_vehicle`   
  ADD  UNIQUE INDEX `NAME_UNQ` (`MODEL`);

ALTER IGNORE  TABLE `pvk`.`pvk_weighing`   
  CHANGE `ACT_PROTOKOL_NUMBER` `ACT_PROTOKOL_NUMBER` VARCHAR(255) NULL;

ALTER TABLE `pvk`.`pvk_weighing`   
  ADD  INDEX `PROTOCOL_DATA_IND` (`ACT_PROTOCOL_DATE`);

ALTER TABLE `pvk`.`pvk_attr_values`   
  ADD COLUMN `ABBR` VARCHAR(200) NULL AFTER `NAME`;
  
ALTER TABLE `pvk`.`pvk_permission_axis_load`   
  ADD COLUMN `COUNT_AXIS_MIN` INT NULL AFTER `PERMISSION_VALUE_3`,
  ADD COLUMN `COUNT_AXIS_MAX` INT NULL AFTER `COUNT_AXIS_MIN`;
