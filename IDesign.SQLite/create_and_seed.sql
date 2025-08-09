-- SQLite script to create and seed Countries and Cities
DROP TABLE IF EXISTS Cities;
DROP TABLE IF EXISTS Countries;

CREATE TABLE Countries (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    Name TEXT NOT NULL
);

CREATE TABLE Cities (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    Name TEXT NOT NULL,
    CountryId INTEGER NOT NULL,
    FOREIGN KEY (CountryId) REFERENCES Countries(Id) ON DELETE CASCADE
);

-- Seed data
INSERT INTO Countries (Name) VALUES ('USA');
INSERT INTO Countries (Name) VALUES ('Canada');

INSERT INTO Cities (Name, CountryId) VALUES ('New York', 1);
INSERT INTO Cities (Name, CountryId) VALUES ('Los Angeles', 1);
INSERT INTO Cities (Name, CountryId) VALUES ('Toronto', 2);
INSERT INTO Cities (Name, CountryId) VALUES ('Vancouver', 2);
