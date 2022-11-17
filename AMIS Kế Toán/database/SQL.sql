/*DEPARTMENT*/
/*Author: Nguyễn Bá Hải*/
CREATE TABLE `misaamisketoan.nbhhai.gpbl_development`.department (
  DepartmentID char(36) NOT NULL DEFAULT '' COMMENT 'ID Đơn Vị',
  DepartmentCode varchar(20) NOT NULL DEFAULT '' COMMENT 'Mã Phòng Ban	',
  DepartmentName varchar(100) NOT NULL DEFAULT '' COMMENT 'Tên Phòng Ban',
  Description varchar(255) DEFAULT NULL COMMENT 'Mô Tả',
  CreatedDate datetime DEFAULT NULL COMMENT 'Ngày tạo',
  CreatedBy varchar(100) DEFAULT NULL COMMENT 'Người Tạo',
  ModifiedDate datetime DEFAULT NULL COMMENT 'Ngày Chỉnh Sửa',
  ModifiedBy varchar(100) DEFAULT NULL COMMENT 'Người Sửa',
  PRIMARY KEY (DepartmentID)
)
ENGINE = INNODB,
AVG_ROW_LENGTH = 2730,
CHARACTER SET utf8mb4,
COLLATE utf8mb4_general_ci,
COMMENT = 'Bảng phòng ban';

ALTER TABLE `misaamisketoan.nbhhai.gpbl_development`.department
ADD UNIQUE INDEX DepartmentCode (DepartmentCode);

/*EMPLOYEE*/
/*Author: Nguyễn Bá Hải*/
CREATE TABLE `misaamisketoan.nbhhai.gpbl_development`.employee (
  EmployeeID char(36) NOT NULL DEFAULT '' COMMENT 'ID Nhân Viên',
  EmployeeCode varchar(20) NOT NULL DEFAULT '' COMMENT 'Mã Nhân Viên',
  EmployeeName varchar(100) NOT NULL DEFAULT '' COMMENT 'Tên Nhân Viên',
  DateOfBirth date DEFAULT NULL COMMENT 'Ngày Sinh',
  Gender tinyint DEFAULT NULL COMMENT 'Giới Tính(0: Nam, 1: Nữ, 2: Khác)',
  IdentityNumber varchar(25) DEFAULT NULL COMMENT 'Số CMND/CCCD',
  IdentityDate date DEFAULT NULL COMMENT 'Ngày Cấp',
  IdentityPlace varchar(255) DEFAULT NULL COMMENT 'Nơi Cấp',
  Address varchar(255) DEFAULT NULL COMMENT 'Địa Chỉ',
  PhoneNumber varchar(50) DEFAULT NULL COMMENT 'Số ĐT Di Động',
  TelephoneNumber varchar(50) DEFAULT NULL COMMENT 'Số ĐT Cố Định',
  Email varchar(100) DEFAULT NULL COMMENT 'Email',
  BankAccountNumber varchar(25) DEFAULT NULL COMMENT 'TK Ngân Hàng',
  BankName varchar(255) DEFAULT NULL COMMENT 'Tên Ngân Hàng',
  BankProvinceName varchar(255) DEFAULT NULL COMMENT 'Chi Nhánh',
  DepartmentID char(36) NOT NULL DEFAULT '' COMMENT 'ID Đơn Vị',
  PositionName varchar(255) DEFAULT '' COMMENT 'Chức Danh',
  CreatedDate datetime DEFAULT NULL COMMENT 'Ngày Tạo',
  CreatedBy varchar(100) DEFAULT NULL COMMENT 'Người Tạo',
  ModifiedDate datetime DEFAULT NULL COMMENT 'Ngày Chỉnh Sửa',
  ModifiedBy varchar(100) DEFAULT NULL COMMENT 'Người Chỉnh Sửa',
  PRIMARY KEY (EmployeeID)
)
ENGINE = INNODB,
AVG_ROW_LENGTH = 501,
CHARACTER SET utf8mb4,
COLLATE utf8mb4_general_ci,
COMMENT = 'Bảng nhân viên';

ALTER TABLE `misaamisketoan.nbhhai.gpbl_development`.employee
ADD UNIQUE INDEX EmployeeCode (EmployeeCode);

ALTER TABLE `misaamisketoan.nbhhai.gpbl_development`.employee
ADD INDEX IX_employee_EmployeeName (EmployeeName);

ALTER TABLE `misaamisketoan.nbhhai.gpbl_development`.employee
ADD INDEX IX_employee_PhoneNumber (PhoneNumber);

ALTER TABLE `misaamisketoan.nbhhai.gpbl_development`.employee
ADD CONSTRAINT FK_employee_department_DepartmentID FOREIGN KEY (DepartmentID)
REFERENCES `misaamisketoan.nbhhai.gpbl_development`.department (DepartmentID);

/*STORE PROCEDURE*/
/*Xóa 1 bản ghi*/
CREATE DEFINER = 'root'@'localhost'
PROCEDURE `misaamisketoan.nbhhai.gpbl_development`.Proc_employee_Delete(IN `@EmployeeID` CHAR(36))
  COMMENT '-- Author:        NBHAI
-- Created date:  29/10/2022
-- Description:   Xóa 1 nhân viên theo ID
-- Modified by:
-- Code chạy thử: CALL `misaamisketoan.nbhhai.gpbl_development`.Proc_employee_DeleteEmployeeByID("GUID");'
BEGIN
  DELETE FROM employee 
  WHERE `@EmployeeID` = employee.EmployeeID;  
END

/*STORE PROCEDURE*/
/*Xóa nhiều bản ghi*/
CREATE DEFINER = 'root'@'localhost'
PROCEDURE `misaamisketoan.nbhhai.gpbl_development`.Proc_employee_DeleteMany(IN `@ArrayID` TEXT)
  COMMENT '-- Author:        NBHAI
-- Created date:  03/11/2022
-- Description: Xóa nhiều bản ghi
-- Modified by:
-- Code chạy thử: CALL `misaamisketoan.nbhhai.gpbl_development`.Proc_employee_DeleteMany((''"bd7f51c0-6553-11ed-9600-08979872ab9b", "7ba5c915-6553-11ed-9600-08979872ab9b", "78475f75-6553-11ed-9600-08979872ab9b"''));'
BEGIN
  SET @query = CONCAT('DELETE FROM  employee e WHERE e.EmployeeID IN (', `@ArrayID`, ')');
  PREPARE del FROM @query;
  EXECUTE del;
  DEALLOCATE PREPARE del;
END

/*STORE PROCEDURE*/
/*Lấy toàn bộ bản ghi có lọc*/
CREATE DEFINER = 'root'@'localhost'
PROCEDURE `misaamisketoan.nbhhai.gpbl_development`.Proc_employee_GetAll(IN `@Filter` VARCHAR(255))
  COMMENT '-- Author:        NBHAI
-- Created date:  28/10/2022
-- Description:   Lấy danh sách tất cả nhân viên
-- Modified by:
-- Code chạy thử: CALL `misaamisketoan.nbhhai.gpbl_development`.Proc_employee_GetAllEmployees(filter);'
BEGIN
  IF IFNULL(`@Filter`, '') = '' THEN
    SET `@Filter` = '"%%"';
  ELSE
    SET `@Filter` = CONCAT('\'%', `@Filter`, '%\'');
  END IF;

  SET @query = CONCAT('SELECT e.EmployeeID,
    e.EmployeeCode,
    e.EmployeeName,
    e.DateOfBirth,
    e.Gender,
    e.PhoneNumber,
    e.IdentityNumber,
    e.Email,
    e.BankAccountNumber,
    e.BankName,
    e.BankProvinceName,
    e.DepartmentID,
    e.PositionName,
    e.CreatedDate
  FROM employee e
  WHERE e.EmployeeName LIKE ', `@Filter`, ' OR e.EmployeeCode LIKE ', `@Filter`, ' OR e.PhoneNumber LIKE ', `@Filter`, ' ORDER BY e.CreatedDate DESC ');
  PREPARE pagingQuery FROM @query;
  EXECUTE pagingQuery;
  DEALLOCATE PREPARE pagingQuery;
END


/*STORE PROCEDURE*/
/*Lấy mã lớn nhất*/
CREATE DEFINER = 'root'@'localhost'
PROCEDURE `misaamisketoan.nbhhai.gpbl_development`.Proc_employee_GetBiggestID()
  COMMENT '-- Author:        NBHAI
-- Created date:  29/10/2022
-- Description:  Lấy mã lớn nhất
-- Modified by:
-- Code chạy thử: CALL Proc_employee_GetID();'
BEGIN
  SELECT
    MAX(e.EmployeeCode)
  FROM employee e;
END

/*STORE PROCEDURE*/
/*Lấy bản ghi theo id*/
CREATE DEFINER = 'root'@'localhost'
PROCEDURE `misaamisketoan.nbhhai.gpbl_development`.Proc_employee_GetByID(IN `@EmployeeID` CHAR(36))
  COMMENT '-- Author:        NBHAI
-- Created date:  28/10/2022
-- Description:   Lấy chi tiết 1 nhân viên theo ID
-- Modified by:
-- Code chạy thử: CALL `misaamisketoan.nbhhai.gpbl_development`.Proc_employee_GetEmployeeByID("GUID");'
BEGIN
  SELECT *
  FROM employee e
  JOIN department d ON e.DepartmentID = d.DepartmentID
  WHERE `@EmployeeID` = e.EmployeeID;
END

/*STORE PROCEDURE*/
/*Lọc và phân trang*/
CREATE DEFINER = 'root'@'localhost'
PROCEDURE `misaamisketoan.nbhhai.gpbl_development`.Proc_employee_GetPaging(IN `@Limit` INT, IN `@Offset` INT, IN `@Filter` VARCHAR(255))
  COMMENT '-- Author:        NBHAI
-- Created date:  28/10/2022
-- Description:   Lấy chi tiết 1 nhân viên theo ID
-- Modified by:
-- Code chạy thử: CALL `misaamisketoan.nbhhai.gpbl_development`.Proc_employee_GetByFiltering(limit,offset,filter);'
BEGIN
  IF IFNULL(`@Filter`, '') = '' THEN
    SET `@Filter` = '"%%"';
  ELSE
    SET `@Filter` = CONCAT('\'%', `@Filter`, '%\'');
  END IF;

  SET @query = CONCAT('SELECT e.EmployeeID,
    e.EmployeeCode,
    e.EmployeeName,
    e.DateOfBirth,
    e.Gender,
    e.PhoneNumber,
    e.IdentityNumber,
    e.Email,
    e.BankAccountNumber,
    e.BankName,
    e.BankProvinceName,
    e.DepartmentID,
    e.PositionName,
    e.CreatedDate
  FROM employee e
  WHERE e.EmployeeName LIKE ', `@Filter`, ' OR e.EmployeeCode LIKE ', `@Filter`, ' OR e.PhoneNumber LIKE ', `@Filter`, ' ORDER BY e.CreatedDate DESC ', ' LIMIT ', `@Limit`, ' OFFSET ', `@Offset`);
  PREPARE pagingQuery FROM @query;
  EXECUTE pagingQuery;
  DEALLOCATE PREPARE pagingQuery;
END

/*STORE PROCEDURE*/
/*Thêm 1 bản ghi*/
CREATE DEFINER = 'root'@'localhost'
PROCEDURE `misaamisketoan.nbhhai.gpbl_development`.Proc_employee_Insert(IN `@EmployeeCode` VARCHAR(20), IN `@EmployeeName` VARCHAR(100), IN `@DateOfBirth` DATE, IN `@Gender` TINYINT, IN `@IdentityNumber` VARCHAR(25), IN `@IdentityDate` DATE, IN `@IdentityPlace` VARCHAR(255), IN `@Address` VARCHAR(255), IN `@PhoneNumber` VARCHAR(50), IN `@TelephoneNumber` VARCHAR(50), IN `@Email` VARCHAR(100), IN `@BankAccountNumber` VARCHAR(25), IN `@BankName` VARCHAR(255), IN `@BankProvinceName` VARCHAR(255), IN `@DepartmentID` CHAR(36), IN `@PositionName` VARCHAR(255))
  COMMENT '-- Author:        NBHAI
-- Created date:  29/10/2022
-- Description:  Thêm 1 nhân viên
-- Modified by:
-- Code chạy thử: CALL Proc_employee_InsertEmployee("NV0198782", "Nguyen Ba Hung", "2001-08-10", 1,null,"2018-12-12",null,null,null,null,null,null,null,null, "469b3ece-744a-45d5-957d-e8c757976496", "3700cc49-55b5-69ea-4929-a2925c0f334d",NULL,null);'
BEGIN
  INSERT INTO employee (EmployeeID, EmployeeCode, EmployeeName, DateOfBirth, Gender, IdentityNumber, IdentityDate, IdentityPlace, Address, PhoneNumber, TelephoneNumber, Email, BankAccountNumber, BankName, BankProvinceName, DepartmentID, PositionName, CreatedDate, CreatedBy, ModifiedDate, ModifiedBy)
    VALUES (UUID(), `@EmployeeCode`, `@EmployeeName`, `@DateOfBirth`, `@Gender`, `@IdentityNumber`, `@IdentityDate`, `@IdentityPlace`, `@Address`, `@PhoneNumber`, `@TelephoneNumber`, `@Email`, `@BankAccountNumber`, `@BankName`, `@BankProvinceName`, `@DepartmentID`, `@PositionName`, NOW(), "Nguyen Hai", NOW(), "Nguyen Hai");

  SELECT e.EmployeeID FROM employee e WHERE e.EmployeeCode = `@EmployeeCode`;
END

/*STORE PROCEDURE*/
/*Sửa 1 bản ghi*/
CREATE DEFINER = 'root'@'localhost'
PROCEDURE `misaamisketoan.nbhhai.gpbl_development`.Proc_employee_Update(IN `@EmployeeID` CHAR(36), IN `@EmployeeCode` VARCHAR(20), IN `@EmployeeName` VARCHAR(100), IN `@DateOfBirth` DATE, IN `@Gender` TINYINT, IN `@IdentityNumber` VARCHAR(25), IN `@IdentityDate` DATE, IN `@IdentityPlace` VARCHAR(255), IN `@Address` VARCHAR(255), IN `@PhoneNumber` VARCHAR(50), IN `@TelephoneNumber` VARCHAR(50), IN `@Email` VARCHAR(100), IN `@BankAccountNumber` VARCHAR(25), IN `@BankName` VARCHAR(255), IN `@BankProvinceName` VARCHAR(255), IN `@DepartmentID` CHAR(36), IN `@PositionName` VARCHAR(255))
  COMMENT '-- Author:        NBHAI
-- Created date:  29/10/2022
-- Description:  Sửa 1 nhân viên
-- Modified by:
-- Code chạy thử: CALL Proc_employee_ModifyEmployee("7a3b91cd-5769-11ed-818e-08979872ab9b","NV0198782", "Nguyen Ba Hung", "2001-08-10", 1,null,"2018-12-12",null,null,null,null,null,null,null,null, "469b3ece-744a-45d5-957d-e8c757976496", "3700cc49-55b5-69ea-4929-a2925c0f334d",NOW(),null);'
BEGIN
  UPDATE employee e
  SET e.EmployeeCode = `@EmployeeCode`,
      e.EmployeeName = `@EmployeeName`,
      e.DateOfBirth = `@DateOfBirth`,
      e.Gender = `@Gender`,
      e.IdentityNumber = `@IdentityNumber`,
      e.IdentityDate = `@IdentityDate`,
      e.IdentityPlace = `@IdentityPlace`,
      e.Address = `@Address`,
      e.PhoneNumber = `@PhoneNumber`,
      e.TelephoneNumber = `@TelephoneNumber`,
      e.Email = `@Email`,
      e.BankAccountNumber = `@BankAccountNumber`,
      e.BankName = `@BankName`,
      e.BankProvinceName = `@BankProvinceName`,
      e.DepartmentID = `@DepartmentID`,
      e.PositionName = `@PositionName`,
      e.ModifiedDate = NOW(),
      e.ModifiedBy = "Ba Hai"
  WHERE e.EmployeeID = `@EmployeeID`;
END

/*STORE PROCEDURE*/
/*Kiểm tra trùng mã*/
CREATE DEFINER = 'root'@'localhost'
PROCEDURE `misaamisketoan.nbhhai.gpbl_development`.Proc_employee_CheckDuplicateCode(IN `@EmployeeCode` VARCHAR(20))
BEGIN
  SELECT COUNT(e.EmployeeCode) 
  FROM employee e
  WHERE e.EmployeeCode = `@EmployeeCode`;
END