USE [master]
go
IF EXISTS (SELECT name FROM sys.databases WHERE name = N'PROJECT_PRU')
BEGIN
	USE master;
	ALTER DATABASE PROJECT_PRU SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
	DROP DATABASE PROJECT_PRU;
END

CREATE DATABASE PROJECT_PRU

GO

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
	[categories] varchar(255),
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
	video varchar(255),
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
	quizz_id bigint FOREIGN KEY REFERENCES quizzes(id),
    answer int,
	is_deleted bit
	)

-- Users
INSERT INTO [dbo].[users] (email, password, role, username) VALUES ('admin@gmail.com', '123456', 'ADMIN', 'KCS');

INSERT INTO users (email, password, role, username, is_deleted)
VALUES 
('user1@example.com', '$2a$11$b8DOklxbErjeahwaYEQ.q.BOcx4drrEqK2NMYU6D1Ox2EKFO2ov5i', 'Student', 'user1', 0),
('user2@example.com', '$2a$11$b8DOklxbErjeahwaYEQ.q.BOcx4drrEqK2NMYU6D1Ox2EKFO2ov5i', 'Student', 'user2', 0),
('user3@example.com', '$2a$11$b8DOklxbErjeahwaYEQ.q.BOcx4drrEqK2NMYU6D1Ox2EKFO2ov5i', 'Student', 'user3', 0),
('user4@example.com', '$2a$11$b8DOklxbErjeahwaYEQ.q.BOcx4drrEqK2NMYU6D1Ox2EKFO2ov5i', 'Student', 'user4', 0),
('user5@example.com', '$2a$11$b8DOklxbErjeahwaYEQ.q.BOcx4drrEqK2NMYU6D1Ox2EKFO2ov5i', 'Student', 'user5', 0),
('user6@example.com', '$2a$11$b8DOklxbErjeahwaYEQ.q.BOcx4drrEqK2NMYU6D1Ox2EKFO2ov5i', 'Student', 'user6', 0),
('lecturer1@example.com', '$2a$11$b8DOklxbErjeahwaYEQ.q.BOcx4drrEqK2NMYU6D1Ox2EKFO2ov5i', 'Lecture', 'lecturer1', 0),
('lecturer2@example.com', '$2a$11$b8DOklxbErjeahwaYEQ.q.BOcx4drrEqK2NMYU6D1Ox2EKFO2ov5i', 'Lecture', 'lecturer2', 0),
('lecturer3@example.com', '$2a$11$b8DOklxbErjeahwaYEQ.q.BOcx4drrEqK2NMYU6D1Ox2EKFO2ov5i', 'Lecture', 'lecturer3', 0),
('lecturer4@example.com', '$2a$11$b8DOklxbErjeahwaYEQ.q.BOcx4drrEqK2NMYU6D1Ox2EKFO2ov5i', 'Lecture', 'lecturer4', 0);


-- Courses
INSERT INTO [dbo].[courses] (user_id, title, thumbnail, categories, description, price, created_at, is_actived, enrol_nums) VALUES 
(8, 'Introduction to HTML and CSS', 'https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcTkfaWZXo9v8ltwKPSXKwKcTXYWCcV49Pt7pw&s','htlm and css','Learn the basics of HTML and CSS to create and style web pages.', 150.00, GETDATE(), 1, 0),
(9, 'JavaScript for Beginners', 'https://static.skillshare.com/uploads/video/thumbnails/0ab63be061d2a2051fc5a20337d2bc7f/original','java', 'Start your journey with JavaScript and learn how to add interactivity to your web pages.', 200.00, GETDATE(), 1, 0),
(10,'Python Programming Fundamentals', 'https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcQvoCDSEmRLejWRBCidegBZHK_rTtT7LyWc9A&s', 'python','An introduction to Python programming covering basic concepts and syntax.', 250.00, GETDATE(), 1, 0),
(11, 'Advanced SQL Queries', 'https://i.ytimg.com/vi/M-55BmjOuXY/hqdefault.jpg', 'sql', 'Master the art of writing complex SQL queries to manipulate and retrieve data.', 300.00, GETDATE(), 1, 0),
(8, 'Web Development with Django', 'https://cdn.educba.com/academy/wp-content/uploads/2017/10/Python-and-Django-for-Web-Development.jpg', 'python', 'Build dynamic web applications using the Django framework in Python.', 350.00, GETDATE(), 1, 0),
(9, 'ReactJS for Front-End Development', 'https://insights.daffodilsw.com/hubfs/Archna/Reactjs%20application%20development.jpg', 'ReactJS', 'Learn how to build modern web applications using ReactJS.', 400.00, GETDATE(), 1, 0),
(10, 'Network Security Basics', 'https://content.nordlayer.com/uploads/network_security_basics_7e0ba955f6.webp', 'network', 'Understand the fundamentals of network security to protect against cyber threats.', 450.00, GETDATE(), 1, 0),
(11, 'DevOps Essentials', 'https://images.ctfassets.net/wfutmusr1t3h/4ez2WNMQR71INMMV61qY45/c051e8c0717eea08136f110dc32c82e6/The_eight_stages_of_a_successful_DevOps_workflow.png', 'DevOps', 'Learn the key concepts of DevOps and how to implement them in your organization.', 500.00, GETDATE(), 1, 0),
(9, 'Mobile App Development with Flutter', 'https://cdn.prod.website-files.com/5f841209f4e71b2d70034471/6078b650748b8558d46ffb7f_Flutter%20app%20development.png', 'flutter framework', 'Create cross-platform mobile applications using the Flutter framework.', 550.00, GETDATE(), 1, 0),
(8, 'Machine Learning with Python', 'https://pyimagesearch.com/wp-content/uploads/2019/01/python_ml_header.png', 'python', 'Dive into machine learning concepts and techniques using Python.', 600.00, GETDATE(), 1, 0);

INSERT INTO courses (user_id, title, thumbnail, categories, description, price, created_at, updated_at, is_actived, enrol_nums, is_deleted)
VALUES
(4, 'User Research for User', 'https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcTkfaWZXo9v8ltwKPSXKwKcTXYWCcV49Pt7pw&s', 'Research', 'Learn about user research techniques.', 29.99, GETDATE(), GETDATE(), 1, 50, 0),
(4, 'Web Development Fundamentals', 'https://content.nordlayer.com/uploads/network_security_basics_7e0ba955f6.webp', 'Web Development', 'Introductory course on web development.', 19.99, GETDATE(), GETDATE(), 1, 80, 0),
(4, 'Data Analysis with Python', 'https://pyimagesearch.com/wp-content/uploads/2019/01/python_ml_header.png', 'Data Science', 'Learn data analysis using Python.', 39.99, GETDATE(), GETDATE(), 1, 60, 0);

-- enroll course
INSERT INTO enroled_courses (user_id, course_id, created_at, updated_at, is_deleted)
VALUES
(2, 1, GETDATE(), GETDATE(), 0),
(3, 2, GETDATE(), GETDATE(), 0),
(4, 3, GETDATE(), GETDATE(), 0),
(5, 4, GETDATE(), GETDATE(), 0),
(6, 5, GETDATE(), GETDATE(), 0),
(7, 6, GETDATE(), GETDATE(), 0),
(5, 7, GETDATE(), GETDATE(), 0),
(4, 7, GETDATE(), GETDATE(), 0),
(2, 7, GETDATE(), GETDATE(), 0),
(2, 6, GETDATE(), GETDATE(), 0),
(4, 5, GETDATE(), GETDATE(), 0),
(5, 6, GETDATE(), GETDATE(), 0),
(6, 5, GETDATE(), GETDATE(), 0),
(7, 4, GETDATE(), GETDATE(), 0),
(2, 2, GETDATE(), GETDATE(), 0);

--explode
INSERT INTO explodes (course_id, content, title, video, is_deleted)
VALUES
(1, 'Lesson1: Introduction to User Research', 'Intro Video', 'zwsPND378OQ', 0),
(1, 'Lesson2: User Persona Creation', 'Persona', '7BJiPyN4zZ0', 0),
(1, 'Lesson3', 'NewThing3', 'JG0pdfdKjgQ', 0),
(1, 'Lesson4', 'NewThing4', 'AzmdwZ6e_aM', 0),
(2, 'Lesson1: Setting Up Your Development Environment', 'Setup', 'ZotVkQDC6mU', 0),
(2, 'Lesson2: Setting Up Your Development Environment', 'Setup', 'ZotVkQDC6mU', 0),
(2, 'Lesson3; Setting Up Your Development Environment', 'Setup', 'ZotVkQDC6mU', 0),
(3, 'Python Basics', 'Python Intro', 'LYnrFSGLCl8', 0);

-- Quizzes for each course

-- Introduction to HTML and CSS
INSERT INTO [dbo].[quizzes] (course_id, question, option_1, option_2, option_3, option_4, answer) VALUES 
(1, 'What does HTML stand for?', 'HyperText Markup Language', 'HyperText Machine Language', 'HyperTool Markup Language', 'HyperText and links Markup Language', 1),
(1, 'Which tag is used to create a hyperlink in HTML?', '<link>', '<a>', '<href>', '<hyperlink>', 2),
(1, 'What does CSS stand for?', 'Cascading Style Sheets', 'Colorful Style Sheets', 'Computer Style Sheets', 'Creative Style Sheets', 1),
(1, 'Which property is used to change the background color in CSS?', 'color', 'bgcolor', 'background-color', 'back-color', 3),
(1, 'Which tag is used to create a paragraph in HTML?', '<p>', '<para>', '<paragraph>', '<text>', 1),
(1, 'How do you insert a comment in CSS?', '// comment', '/* comment */', '<!-- comment -->', ':: comment ::', 2),
(1, 'Which CSS property is used to change the text color?', 'text-color', 'font-color', 'color', 'text-style', 3),
(1, 'What is the correct HTML element for the largest heading?', '<heading>', '<h6>', '<head>', '<h1>', 4),
(1, 'How do you add a background color for all <h1> elements in CSS?', 'h1 {background-color: #ffffff;}', 'h1.all {background-color: #ffffff;}', 'all.h1 {background-color: #ffffff;}', 'h1 {bgcolor: #ffffff;}', 1),
(1, 'Which HTML attribute is used to define inline styles?', 'class', 'font', 'styles', 'style', 4);

-- JavaScript for Beginners
INSERT INTO [dbo].[quizzes] (course_id, question, option_1, option_2, option_3, option_4, answer) VALUES 
(2, 'Inside which HTML element do we put the JavaScript?', '<js>', '<scripting>', '<script>', '<javascript>', 3),
(2, 'What is the correct syntax for referring to an external script called "xxx.js"?', '<script name="xxx.js">', '<script src="xxx.js">', '<script href="xxx.js">', '<script file="xxx.js">', 2),
(2, 'How do you create a function in JavaScript?', 'function = myFunction()', 'function:myFunction()', 'function myFunction()', 'func myFunction()', 3),
(2, 'How do you call a function named "myFunction"?', 'call myFunction()', 'myFunction()', 'call function myFunction()', 'exec myFunction()', 2),
(2, 'How to write an IF statement in JavaScript?', 'if i == 5 then', 'if i = 5', 'if (i == 5)', 'if i = 5 then', 3),
(2, 'Which event occurs when the user clicks on an HTML element?', 'onchange', 'onmouseclick', 'onmouseover', 'onclick', 4),
(2, 'How do you declare a JavaScript variable?', 'variable carName;', 'v carName;', 'var carName;', 'v:carName;', 3),
(2, 'Which operator is used to assign a value to a variable?', 'x', '*', '=', '-', 3),
(2, 'What will the following code return: Boolean(10 > 9)', 'true', 'NaN', 'false', 'null', 1),
(2, 'Which JavaScript method is used to write into an alert box?', 'window.alert()', 'message()', 'alert()', 'msg()', 3);

-- Python Programming Fundamentals
INSERT INTO [dbo].[quizzes] (course_id, question, option_1, option_2, option_3, option_4, answer) VALUES 
(3, 'Which of the following is a correct variable assignment in Python?', 'x = 5', 'let x = 5', 'int x = 5', 'x := 5', 1),
(3, 'How do you create a function in Python?', 'function myFunction()', 'def myFunction():', 'create myFunction():', 'func myFunction()', 2),
(3, 'Which keyword is used to start a loop in Python?', 'for', 'repeat', 'loop', 'iterate', 1),
(3, 'How do you add a comment in Python?', '// comment', '# comment', '/* comment */', '<!-- comment -->', 2),
(3, 'Which data type is used to store text in Python?', 'Text', 'Char', 'String', 'str', 4),
(3, 'What is the correct file extension for Python files?', '.pt', '.pyt', '.pyth', '.py', 4),
(3, 'How do you create a list in Python?', 'list = []', 'list = array()', 'list = {}', 'list = list()', 1),
(3, 'How do you create a dictionary in Python?', 'dict = []', 'dict = array()', 'dict = {}', 'dict = dict()', 3),
(3, 'Which function is used to output data to the screen in Python?', 'echo()', 'print()', 'write()', 'output()', 2),
(3, 'Which operator is used for multiplication in Python?', 'x', '*', '×', 'X', 2);

-- Advanced SQL Queries
INSERT INTO [dbo].[quizzes] (course_id, question, option_1, option_2, option_3, option_4, answer) VALUES 
(4, 'Which SQL statement is used to retrieve data from a database?', 'SELECT', 'GET', 'EXTRACT', 'FETCH', 1),
(4, 'Which SQL clause is used to filter records?', 'WHERE', 'FILTER', 'SEARCH', 'SORT', 1),
(4, 'How do you select all columns from a table named "Employees"?', 'SELECT * FROM Employees', 'SELECT all FROM Employees', 'SELECT columns FROM Employees', 'SELECT Employees', 1),
(4, 'Which SQL statement is used to update data in a database?', 'CHANGE', 'UPDATE', 'MODIFY', 'ALTER', 2),
(4, 'Which SQL keyword is used to sort the result-set?', 'ORDER', 'SORT', 'ARRANGE', 'ORDER BY', 4),
(4, 'Which SQL statement is used to delete data from a database?', 'REMOVE', 'DELETE', 'DROP', 'CUT', 2),
(4, 'What does the COUNT function do in SQL?', 'Counts all rows', 'Counts only unique rows', 'Counts only non-null rows', 'Counts only null rows', 1),
(4, 'Which SQL keyword is used to retrieve a maximum value?', 'HIGHEST', 'TOP', 'MAX', 'LIMIT', 3),
(4, 'What does the GROUP BY clause do in SQL?', 'Groups rows that have the same values', 'Sorts the result set', 'Filters the result set', 'Joins multiple tables', 1),
(4, 'Which SQL statement is used to create a new table?', 'CREATE TABLE', 'NEW TABLE', 'ADD TABLE', 'MAKE TABLE', 1);

-- Web Development with Django
INSERT INTO [dbo].[quizzes] (course_id, question, option_1, option_2, option_3, option_4, answer) VALUES 
(5, 'What is Django?', 'A Python library for data analysis', 'A Python web framework', 'A JavaScript framework', 'A database management system', 2),
(5, 'Which command is used to create a new Django project?', 'django startproject', 'django-admin startproject', 'django createproject', 'django-admin createproject', 2),
(5, 'How do you define a model in Django?', 'By creating a class in models.py', 'By creating a function in models.py', 'By creating a class in views.py', 'By creating a function in views.py', 1),
(5, 'Which command is used to apply migrations in Django?', 'django migrate', 'django-admin migrate', 'python manage.py migrate', 'python django migrate', 3),
(5, 'How do you start the Django development server?', 'python manage.py runserver', 'django-admin runserver', 'django startserver', 'python django runserver', 1),
(5, 'What is the purpose of the settings.py file in a Django project?', 'To define URL patterns', 'To define models', 'To configure project settings', 'To create views', 3),
(5, 'Which of the following is a Django template tag?', '{% %}', '{{ }}', '{# #}', '<# #>', 1),
(5, 'How do you create a superuser in Django?', 'django-admin createsuperuser', 'python manage.py createsuperuser', 'django createsuperuser', 'python django createsuperuser', 2),
(5, 'What is the default database used by Django?', 'MySQL', 'PostgreSQL', 'SQLite', 'Oracle', 3),
(5, 'Which file is used to define URL patterns in Django?', 'urls.py', 'routes.py', 'paths.py', 'links.py', 1);

-- ReactJS for Front-End Development
INSERT INTO [dbo].[quizzes] (course_id, question, option_1, option_2, option_3, option_4, answer) VALUES 
(6, 'What is ReactJS?', 'A server-side framework', 'A front-end JavaScript library', 'A database management tool', 'A back-end framework', 2),
(6, 'How do you create a new React application?', 'create-react-app my-app', 'react-create-app my-app', 'new-react-app my-app', 'init-react-app my-app', 1),
(6, 'What is a component in React?', 'A reusable piece of UI', 'A database model', 'A CSS style', 'A server route', 1),
(6, 'Which method is used to render a React component to the DOM?', 'render()', 'ReactDOM.render()', 'React.render()', 'DOM.render()', 2),
(6, 'How do you pass data to a React component?', 'Using state', 'Using props', 'Using parameters', 'Using data()', 2),
(6, 'What is JSX?', 'A JavaScript syntax extension', 'A CSS preprocessor', 'A database query language', 'A testing framework', 1),
(6, 'How do you handle events in React?', 'Using event listeners', 'Using event handlers', 'Using functions', 'Using methods', 2),
(6, 'What is the purpose of the useState hook in React?', 'To manage component state', 'To create components', 'To handle events', 'To fetch data', 1),
(6, 'Which lifecycle method is called after a component is rendered?', 'componentDidMount', 'componentWillMount', 'componentWillUpdate', 'componentDidUpdate', 1),
(6, 'How do you import a component in React?', 'import Component from "./Component"', 'require("./Component")', 'import { Component } from "./Component"', 'import "./Component"', 3);

-- Network Security Basics
INSERT INTO [dbo].[quizzes] (course_id, question, option_1, option_2, option_3, option_4, answer) VALUES 
(7, 'What is a firewall?', 'A hardware device that connects networks', 'A system that monitors and controls incoming and outgoing network traffic', 'A type of malware', 'A software that protects against viruses', 2),
(7, 'Which protocol is used to encrypt web traffic?', 'HTTP', 'FTP', 'SMTP', 'HTTPS', 4),
(7, 'What does VPN stand for?', 'Virtual Private Network', 'Virtual Public Network', 'Visible Private Network', 'Visible Public Network', 1),
(7, 'Which of the following is a type of social engineering attack?', 'DDoS attack', 'Phishing', 'SQL injection', 'Man-in-the-middle attack', 2),
(7, 'What is the purpose of a proxy server?', 'To block malware', 'To encrypt data', 'To act as an intermediary for requests from clients', 'To detect intrusions', 3),
(7, 'What does the term "malware" refer to?', 'Software designed to protect against viruses', 'Software designed to gain unauthorized access or damage a computer system', 'A type of network protocol', 'A hardware component', 2),
(7, 'Which tool is commonly used for network packet analysis?', 'Wireshark', 'Photoshop', 'Excel', 'Slack', 1),
(7, 'What is the function of an IDS?', 'To encrypt data', 'To monitor network traffic for suspicious activity', 'To create backups', 'To filter web content', 2),
(7, 'What does the acronym "DDoS" stand for?', 'Distributed Denial of Service', 'Direct Denial of Service', 'Distributed Data of Service', 'Direct Data of Service', 1),
(7, 'Which of the following is a strong password?', 'password123', '123456', 'qwerty', 'A complex combination of letters, numbers, and symbols', 4);

-- DevOps Essentials
INSERT INTO [dbo].[quizzes] (course_id, question, option_1, option_2, option_3, option_4, answer) VALUES 
(8, 'What does DevOps stand for?', 'Development Operations', 'Development and Operations', 'Developer Operations', 'Deploy Operations', 2),
(8, 'Which tool is used for version control in DevOps?', 'Docker', 'Ansible', 'Jenkins', 'Git', 4),
(8, 'What is continuous integration?', 'A method to integrate data continuously', 'A practice of merging code changes frequently', 'A way to deploy applications automatically', 'A method to build software continuously', 2),
(8, 'Which tool is commonly used for containerization?', 'Jenkins', 'Docker', 'Puppet', 'Git', 2),
(8, 'What is the purpose of Jenkins in DevOps?', 'To automate testing', 'To manage servers', 'To automate the build process', 'To manage databases', 3),
(8, 'Which practice is essential for continuous delivery?', 'Frequent code commits', 'Manual testing', 'Server maintenance', 'Database backup', 1),
(8, 'What does the term "infrastructure as code" refer to?', 'Coding server infrastructure manually', 'Managing and provisioning computing infrastructure using code', 'Writing code for hardware devices', 'Designing physical infrastructure using software', 2),
(8, 'Which tool is used for configuration management?', 'Docker', 'Git', 'Ansible', 'Jenkins', 3),
(8, 'What is the main benefit of using DevOps?', 'Faster software development and delivery', 'Reduced server costs', 'Enhanced hardware performance', 'Improved database management', 1),
(8, 'What does the term "continuous deployment" refer to?', 'Deploying code changes manually', 'Deploying code changes automatically to production', 'Testing code changes continuously', 'Building software continuously', 2);

-- Mobile App Development with Flutter
INSERT INTO [dbo].[quizzes] (course_id, question, option_1, option_2, option_3, option_4, answer) VALUES 
(9, 'What is Flutter?', 'A framework for building mobile applications', 'A database management system', 'A version control system', 'A network security tool', 1),
(9, 'Which language is used to write Flutter apps?', 'JavaScript', 'Java', 'Dart', 'Python', 3),
(9, 'What is a widget in Flutter?', 'A reusable piece of code', 'A type of variable', 'A package manager', 'A database query', 1),
(9, 'How do you create a new Flutter project?', 'flutter create my_project', 'flutter new my_project', 'flutter init my_project', 'flutter start my_project', 1),
(9, 'What is the purpose of the pubspec.yaml file in a Flutter project?', 'To define project settings', 'To list dependencies and assets', 'To manage database connections', 'To configure network settings', 2),
(9, 'Which of the following is a state management solution in Flutter?', 'Redux', 'Provider', 'Bloc', 'All of the above', 4),
(9, 'How do you run a Flutter app on an emulator?', 'flutter run', 'flutter start', 'flutter build', 'flutter init', 1),
(9, 'What is hot reload in Flutter?', 'A feature to restart the app', 'A feature to reload the app state', 'A feature to update the app without restarting', 'A feature to build the app', 3),
(9, 'Which widget is used to create a scrollable list in Flutter?', 'ListView', 'GridView', 'ScrollView', 'ListBuilder', 1),
(9, 'How do you define a route in Flutter?', 'Using the routes parameter in the MaterialApp widget', 'Using the route function', 'Using the navigator widget', 'Using the router function', 1);

-- Machine Learning with Python
INSERT INTO [dbo].[quizzes] (course_id, question, option_1, option_2, option_3, option_4, answer) VALUES 
(10, 'What is machine learning?', 'A type of data analysis', 'A method of programming', 'A type of artificial intelligence', 'A database management system', 3),
(10, 'Which library is commonly used for machine learning in Python?', 'NumPy', 'Pandas', 'Matplotlib', 'Scikit-learn', 4),
(10, 'What is a dataset?', 'A collection of data', 'A machine learning model', 'A type of algorithm', 'A programming language', 1),
(10, 'What is supervised learning?', 'Learning from unlabeled data', 'Learning from labeled data', 'Learning without data', 'Learning from semi-labeled data', 2),
(10, 'Which of the following is an unsupervised learning algorithm?', 'Linear Regression', 'Decision Tree', 'K-means Clustering', 'Logistic Regression', 3),
(10, 'What is a neural network?', 'A type of algorithm', 'A set of interconnected nodes', 'A data structure', 'A programming paradigm', 2),
(10, 'What does the term "overfitting" refer to?', 'A model that performs well on training data but poorly on new data', 'A model that performs poorly on training data', 'A model that fits the data perfectly', 'A model that generalizes well to new data', 1),
(10, 'Which metric is used to evaluate classification models?', 'Mean Squared Error', 'Accuracy', 'R-squared', 'Root Mean Squared Error', 2),
(10, 'What is a confusion matrix?', 'A table that describes the performance of a classification model', 'A table that contains the input data', 'A matrix used for data normalization', 'A method for data visualization', 1),
(10, 'Which of the following is a common activation function in neural networks?', 'Sigmoid', 'Linear', 'Polynomial', 'Radial', 1);

-- history of quiz attempts
INSERT INTO history_quizzes (user_id, quizz_id, answer, is_deleted)
VALUES
(2, 1, 1, 0),
(3, 2, 1, 0),
(2, 1, 1, 0),
(3, 2, 1, 0),
(2, 1, 1, 0),
(3, 2, 1, 0),
(2, 1, 1, 0),
(3, 2, 1, 0),
(2, 1, 1, 0),
(3, 2, 1, 0),
(2, 1, 1, 0),
(3, 2, 1, 0),
(2, 3, 1, 0);
GO

-- CODING TIME
CREATE TRIGGER OnEnrollment 
ON [dbo].[enroled_courses]
AFTER INSERT
AS
BEGIN
	UPDATE [dbo].[courses]
	SET enrol_nums = enrol_nums + 1
	FROM [dbo].[courses] 
	JOIN inserted ON courses.id = inserted.course_id;
END;

GO

CREATE PROC GetPoint
@id_user bigint, @id_course bigint,
@point float out
AS
BEGIN
	DECLARE @total_question int
	DECLARE @right_answer int
	SELECT @total_question = COUNT(*) from history_quizzes hq 
	LEFT JOIN quizzes q on hq.quizz_id = q.id
	LEFT JOIN courses c on c.id = q.course_id
	where c.id = @id_course and hq.user_id = @id_user
	SELECT @right_answer = COUNT(*) from history_quizzes hq 
	LEFT JOIN quizzes q on hq.quizz_id = q.id
	LEFT JOIN courses c on c.id = q.course_id
	where c.id = @id_course and hq.user_id = @id_user and q.answer = hq.answer
    SET	@point = (@right_answer / @total_question) * 10.0;
END

GO

CREATE PROC GetRevenues 
@revenues float out
AS 
BEGIN
	select @revenues = SUM(c.price) from enroled_courses ec
	join courses c on c.id = ec.course_id
END
