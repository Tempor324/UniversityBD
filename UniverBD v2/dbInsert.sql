USE University;

INSERT INTO teachers(teacherName) VALUES ("Тамара Петрова Евгеньевна");
INSERT INTO teachers(teacherName) VALUES ("Евгений Петров Евгеньевич");

INSERT INTO disciplines(disciplineId, disciplineName) VALUES ("1.1","Алгебра");
INSERT INTO disciplines(disciplineId, disciplineName) VALUES ("1.2","Геометрия");
INSERT INTO disciplines(disciplineId, disciplineName) VALUES ("2.1","Русский язык");

INSERT INTO TeacherDisciplines(teacherId, disciplineId) VALUES (1,"1.1");
INSERT INTO TeacherDisciplines(teacherId, disciplineId) VALUES (1,"1.2");
INSERT INTO TeacherDisciplines(teacherId, disciplineId) VALUES (2,"2.1");

INSERT INTO studentGroups(groupId, yearNum, teacherId) VALUES (500, 2, 1);
INSERT INTO studentGroups(groupId, yearNum, teacherId) VALUES (501, 3, 1);
INSERT INTO studentGroups(groupId, yearNum, teacherId) VALUES (502, 2, 2);

INSERT INTO students(studentName, groupId, birthDate) VALUES ("Орлов Николай Юрьевич", 500, "2000-05-05");
INSERT INTO students(studentName, groupId, birthDate) VALUES ("Девяткин Николай Юрьевич", 500, "2000-04-04");
INSERT INTO students(studentName, groupId, birthDate) VALUES ("Девяткин Александр Юрьевич", 502, "2000-04-04");
INSERT INTO students(studentName, groupId, birthDate) VALUES ("Девяткин Александр Валерьевич", 501, "2000-04-04");
INSERT INTO students(studentName, groupId, birthDate) VALUES ("Девяткин Александр Николаевич", 501, "2000-04-04");
INSERT INTO students(studentName, groupId, birthDate) VALUES ("Десяткин Михаил", 502, "2000-11-04");