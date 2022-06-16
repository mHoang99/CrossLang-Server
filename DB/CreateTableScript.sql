CREATE TABLE `user` (
  ID bigint UNSIGNED AUTO_INCREMENT NOT NULL PRIMARY KEY COMMENT 'ID',
  ModifiedBy varchar(100),
  CreatedBy varchar(100),
  ModifiedDate datetime DEFAULT NOW(),
  CreatedDate datetime DEFAULT NOW(),
  Username varchar(50),
  Password varchar(255),
  Avatar varchar(255),
  FullName varchar(100),
  DateOfBirth date,
  Email varchar(100),
  Gender smallint,
  PhoneNumber varchar(20),
  Address text,
  UserPermission bigint COMMENT 'Quyền của người dùng',
  PackagePermission bigint COMMENT 'Quyền của gói',
  RoleID bigint COMMENT 'Vai trò',
  EmployeeID bigint
);

CREATE TABLE `employee` (
  ID bigint UNSIGNED AUTO_INCREMENT NOT NULL PRIMARY KEY COMMENT 'ID',
  ModifiedBy varchar(100),
  CreatedBy varchar(100),
  ModifiedDate datetime DEFAULT NOW(),
  CreatedDate datetime DEFAULT NOW(),
  Description text,
  EmployeeCode varchar(20),
  PersonalImage varchar(500)
);

CREATE TABLE `employee_schedule` (
  ID bigint UNSIGNED AUTO_INCREMENT NOT NULL PRIMARY KEY COMMENT 'ID',
  ModifiedBy varchar(100),
  CreatedBy varchar(100),
  ModifiedDate datetime DEFAULT NOW(),
  CreatedDate datetime DEFAULT NOW(),
  Description text,
  EmployeeID bigint,
  StudentID bigint,
  StartTime datetime,
  EndTime datetime,
  MeetingLink text
);

CREATE TABLE Role (
  ID bigint UNSIGNED AUTO_INCREMENT NOT NULL PRIMARY KEY,
  ModifiedBy varchar(100),
  CreatedBy varchar(100),
  ModifiedDate datetime DEFAULT NOW(),
  CreatedDate datetime DEFAULT NOW(),
  RoleName varchar(50) COMMENT 'Tên vai trò',
  RoleCode varchar(50) COMMENT 'Mã vai trò',
  RolePermission bigint COMMENT 'Quyền của vai trò'
);

CREATE TABLE leader_board_snake (
  ID bigint UNSIGNED AUTO_INCREMENT NOT NULL PRIMARY KEY,
  ModifiedBy varchar(100),
  CreatedBy varchar(100),
  ModifiedDate datetime DEFAULT NOW(),
  CreatedDate datetime DEFAULT NOW(),
  UserID bigint NOT NULL,
  Score bigint NOT NULL
);

CREATE TABLE leader_board_wordament (
  ID bigint UNSIGNED AUTO_INCREMENT NOT NULL PRIMARY KEY,
  ModifiedBy varchar(100),
  CreatedBy varchar(100),
  ModifiedDate datetime DEFAULT NOW(),
  CreatedDate datetime DEFAULT NOW(),
  UserID bigint NOT NULL,
  Score bigint NOT NULL
);

CREATE TABLE leader_board_helicopter (
  ID bigint UNSIGNED AUTO_INCREMENT NOT NULL PRIMARY KEY,
  ModifiedBy varchar(100),
  CreatedBy varchar(100),
  ModifiedDate datetime DEFAULT NOW(),
  CreatedDate datetime DEFAULT NOW(),
  UserID bigint NOT NULL,
  Score bigint NOT NULL
);

CREATE TABLE leader_board_warship (
  ID bigint UNSIGNED AUTO_INCREMENT NOT NULL PRIMARY KEY,
  ModifiedBy varchar(100),
  CreatedBy varchar(100),
  ModifiedDate datetime DEFAULT NOW(),
  CreatedDate datetime DEFAULT NOW(),
  UserID bigint NOT NULL,
  Score bigint NOT NULL
);

CREATE TABLE leader_board_flip (
  ID bigint UNSIGNED AUTO_INCREMENT NOT NULL PRIMARY KEY,
  ModifiedBy varchar(100),
  CreatedBy varchar(100),
  ModifiedDate datetime DEFAULT NOW(),
  CreatedDate datetime DEFAULT NOW(),
  UserID bigint NOT NULL,
  Score bigint NOT NULL
);

CREATE TABLE dictionary_word (
  ID bigint UNSIGNED AUTO_INCREMENT NOT NULL PRIMARY KEY,
  ModifiedBy varchar(100),
  CreatedBy varchar(100),
  ModifiedDate datetime DEFAULT NOW(),
  CreatedDate datetime DEFAULT NOW(),
  Text varchar(255),
  Image varchar(500),
  Pronunciation varchar(255)
 );

CREATE TABLE dictionary_word_translate_vi (
  ID bigint UNSIGNED AUTO_INCREMENT NOT NULL PRIMARY KEY,
  ModifiedBy varchar(100),
  CreatedBy varchar(100),
  ModifiedDate datetime DEFAULT NOW(),
  CreatedDate datetime DEFAULT NOW(),
  DictionaryWordID bigint NOT NULL,
  Translation text,
  WordType smallint,
  Examples json
);

CREATE TABLE dictionary_word_translate_en (
  ID bigint UNSIGNED AUTO_INCREMENT NOT NULL PRIMARY KEY,
  ModifiedBy varchar(100),
  CreatedBy varchar(100),
  ModifiedDate datetime DEFAULT NOW(),
  CreatedDate datetime DEFAULT NOW(),
  DictionaryWordID bigint NOT NULL,
  Translation text,
  WordType smallint,
  Examples json
);


CREATE TABLE dictionary_word_related_mapping(
 ID bigint UNSIGNED AUTO_INCREMENT NOT NULL PRIMARY KEY,
  ModifiedBy varchar(100),
  CreatedBy varchar(100),
  ModifiedDate datetime DEFAULT NOW(),
  CreatedDate datetime DEFAULT NOW(),
  DictionaryWordID bigint NOT NULL,
  DictionaryWordText varchar(255),
  Context text,
  RelatedWords Text,
  WordType smallint,
  Type smallint COMMENT '1: Synonym, 2: Antonym'
);

CREATE TABLE dictionary_word_addition_vi(
  ID bigint UNSIGNED AUTO_INCREMENT NOT NULL PRIMARY KEY,
  ModifiedBy varchar(100),
  CreatedBy varchar(100),
  ModifiedDate datetime DEFAULT NOW(),
  CreatedDate datetime DEFAULT NOW(),
  DictionaryWordID bigint NOT NULL,
  WordType smallint,
  JsonContent json
);

CREATE TABLE dictionary_word_addition_en(
  ID bigint UNSIGNED AUTO_INCREMENT NOT NULL PRIMARY KEY,
  ModifiedBy varchar(100),
  CreatedBy varchar(100),
  ModifiedDate datetime DEFAULT NOW(),
  CreatedDate datetime DEFAULT NOW(),
  DictionaryWordID bigint NOT NULL,
  WordType smallint,
  JsonContent json
);


CREATE TABLE flash_card (
  ID bigint UNSIGNED AUTO_INCREMENT NOT NULL PRIMARY KEY,
  ModifiedBy varchar(100),
  CreatedBy varchar(100),
  ModifiedDate datetime DEFAULT NOW(),
  CreatedDate datetime DEFAULT NOW(),
  UserID bigint NOT NULL,
  DictionaryWordID bigint NOT NULL,
  FlashCardCollectionID bigint,
  IsFinished bit(1)
);

CREATE TABLE flash_card_collection (
  ID bigint UNSIGNED AUTO_INCREMENT NOT NULL PRIMARY KEY,
  ModifiedBy varchar(100),
  CreatedBy varchar(100),
  ModifiedDate datetime DEFAULT NOW(),
  CreatedDate datetime DEFAULT NOW(),
  UserID bigint NOT NULL,
  IsFinished bit(1),
  Title varchar(255)
);




-- SELECT * FROM dictionary_word_synonym_mapping dwsm
-- JOIN  dictionary_word dw1 ON dw1.ID = dwsm.DictionaryWordID 
-- JOIN dictionary_word dw2 ON dw2.ID = dwsm.DictionaryWord2ID 
-- WHERE dwsm.DictionaryWordID = @ID OR dwsm.DictionaryWord2ID = @ID;

CREATE TABLE lesson (
  ID bigint UNSIGNED AUTO_INCREMENT NOT NULL PRIMARY KEY,
  ModifiedBy varchar(100),
  CreatedBy varchar(100),
  ModifiedDate datetime DEFAULT NOW(),
  CreatedDate datetime DEFAULT NOW(),
  Title varchar(255),
  Description text,
  VideoLinks text,
  Level int,
  DocumentLinks text,
  Content longtext,
  LessonCategoryID bigint,
  Thumbnail varchar(500)
);

CREATE TABLE lesson_category (
  ID bigint UNSIGNED AUTO_INCREMENT NOT NULL PRIMARY KEY,
  ModifiedBy varchar(100),
  CreatedBy varchar(100),
  ModifiedDate datetime DEFAULT NOW(),
  CreatedDate datetime DEFAULT NOW(),
  Name varchar(255),
  Description text,
  PackagePermission bigint,
  Image varchar(500)
);


CREATE TABLE lesson_dictionary_word_mapping (
  ID bigint UNSIGNED AUTO_INCREMENT NOT NULL PRIMARY KEY,
  ModifiedBy varchar(100),
  CreatedBy varchar(100),
  ModifiedDate datetime DEFAULT NOW(),
  CreatedDate datetime DEFAULT NOW(),
  LessonID long,
  DictionaryWordID long,
  DictionaryWordText varchar(255)
);

CREATE TABLE exercise (
  ID bigint UNSIGNED AUTO_INCREMENT NOT NULL PRIMARY KEY,
  ModifiedBy varchar(100),
  CreatedBy varchar(100),
  ModifiedDate datetime DEFAULT NOW(),
  CreatedDate datetime DEFAULT NOW(),
  Name varchar(255),
  Description longtext,
  Type smallint,
  IsShuffle bit(1)
);

CREATE TABLE question (
  ID bigint UNSIGNED AUTO_INCREMENT NOT NULL PRIMARY KEY,
  ModifiedBy varchar(100),
  CreatedBy varchar(100),
  ModifiedDate datetime DEFAULT NOW(),
  CreatedDate datetime DEFAULT NOW(),
  Content text,
  Answers text,
  Options text,
  Type smallint
);

CREATE TABLE exercise_question_mapping (
  ID bigint UNSIGNED AUTO_INCREMENT NOT NULL PRIMARY KEY,
  ModifiedBy varchar(100),
  CreatedBy varchar(100),
  ModifiedDate datetime DEFAULT NOW(),
  CreatedDate datetime DEFAULT NOW(),
  ExerciseID bigint not null,
  QuestionID bigint NOT NULL
);

CREATE TABLE user_lesson_status (
  ID bigint UNSIGNED AUTO_INCREMENT NOT NULL PRIMARY KEY,
  ModifiedBy varchar(100),
  CreatedBy varchar(100),
  ModifiedDate datetime DEFAULT NOW(),
  CreatedDate datetime DEFAULT NOW(),
  UserID bigint NOT NULL,
  LessonID bigint NOT NULL,
  IsFinished bit(1)
);

CREATE TABLE user_exercise_status (
  ID bigint UNSIGNED AUTO_INCREMENT NOT NULL PRIMARY KEY,
  ModifiedBy varchar(100),
  CreatedBy varchar(100),
  ModifiedDate datetime DEFAULT NOW(),
  CreatedDate datetime DEFAULT NOW(),
  UserID bigint,
  ExerciseID bigint,
  StartTime datetime,
  EndTime datetime,
  CorrectAnswers int,
  TotalAnswers int
);

CREATE TABLE refresh_token (
  ID bigint UNSIGNED AUTO_INCREMENT NOT NULL PRIMARY KEY,
  ModifiedBy varchar(100),
  CreatedBy varchar(100),
  ModifiedDate datetime DEFAULT NOW(),
  CreatedDate datetime DEFAULT NOW(),
  HashedValue varchar(1000),
  UserID bigint
);