use master
create database PROJECT_PRU
use PROJECT_PRU

CREATE TABLE users (
    id bigint NOT NULL PRIMARY KEY identity,
    email varchar(255),
    [password] varchar(255),
	[role] varchar(100),
	[username] varchar(255),
	is_deleted bit
);

CREATE TABLE tokens (
    id bigint NOT NULL PRIMARY KEY identity,
	user_id bigint FOREIGN KEY REFERENCES users(id),
    [type] varchar(100),
    [expired_date] DATETIME,
	is_deleted bit
	)

CREATE TABLE courses (
    id bigint NOT NULL PRIMARY KEY identity,
	user_id bigint FOREIGN KEY REFERENCES users(id),
    [title] varchar(255),
	[thumbnail] varchar(255),
	[description] varchar(255),
	[price] decimal(10,2),
    [created_at] DATETIME,
	[updated_at] DATETIME ,
	is_actived bit,
	enrol_nums bigint,
	is_deleted bit
	)

	CREATE TABLE enroled_courses (
    id bigint NOT NULL PRIMARY KEY identity,
	user_id bigint FOREIGN KEY REFERENCES users(id),
	course_id bigint FOREIGN KEY REFERENCES courses(id),
    [created_at] DATETIME,
	[updated_at] DATETIME,
	is_deleted bit
	)

	CREATE TABLE explodes(
	id bigint NOT NULL PRIMARY KEY identity,
	course_id bigint FOREIGN KEY REFERENCES courses(id),
	content varchar(255),
	title varchar(255),
	is_deleted bit
	)

	CREATE TABLE quizzes(
	id bigint NOT NULL PRIMARY KEY identity,
	course_id bigint FOREIGN KEY REFERENCES courses(id),
	question varchar(255),
	option_1 varchar(255),
	option_2 varchar(255),
	option_3 varchar(255),
	option_4 varchar(255),
	answer int,
	is_deleted bit
	)

	CREATE TABLE history_quizzes (
    id bigint NOT NULL PRIMARY KEY identity,
	user_id bigint FOREIGN KEY REFERENCES users(id),
	course_id bigint FOREIGN KEY REFERENCES courses(id),
    answer int,
	is_deleted bit
	)