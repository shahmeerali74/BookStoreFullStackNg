-- Authors
INSERT INTO Author (Id, AuthorName) VALUES
(1, 'J.K. Rowling'),
(2, 'George R.R. Martin'),
(3, 'J.R.R. Tolkien'),
(4, 'Agatha Christie'),
(5, 'Stephen King'),
(6, 'Isaac Asimov'),
(7, 'Arthur Conan Doyle'),
(8, 'Suzanne Collins'),
(9, 'Dan Brown'),
(10, 'Harper Lee');

-- Genres
INSERT INTO Genre (Id, GenreName) VALUES
(1, 'Fantasy'),
(2, 'Science Fiction'),
(3, 'Mystery'),
(4, 'Thriller'),
(5, 'Romance'),
(6, 'Historical Fiction'),
(7, 'Horror'),
(8, 'Adventure'),
(9, 'Drama'),
(10, 'Non-Fiction');

-- Books  
-- Since your Book table has no GenreId column, I will fill only the existing columns:
INSERT INTO Book (Id, Description, ImageUrl, Price, PublishedYear, Title) VALUES
(1, 'A young wizard begins his magical journey.', 'harrypotter.jpg', 19.99, 1997, 'Harry Potter and the Sorcerer''s Stone'),
(2, 'A battle for the Iron Throne in a fantasy realm.', 'got.jpg', 24.99, 1996, 'A Game of Thrones'),
(3, 'A hobbit sets out on an epic adventure.', 'hobbit.jpg', 15.99, 1937, 'The Hobbit'),
(4, 'Detective Hercule Poirot solves a murder mystery.', 'orientexpress.jpg', 12.99, 1934, 'Murder on the Orient Express'),
(5, 'A haunted hotel drives its caretaker insane.', 'shining.jpg', 14.99, 1977, 'The Shining'),
(6, 'A sci-fi classic about the fall of a galactic empire.', 'foundation.jpg', 18.99, 1951, 'Foundation'),
(7, 'The first case of Sherlock Holmes and Dr. Watson.', 'scarlet.jpg', 10.99, 1887, 'A Study in Scarlet'),
(8, 'A dystopian tale of survival in deadly games.', 'hungergames.jpg', 16.99, 2008, 'The Hunger Games'),
(9, 'A symbologist uncovers secrets of the Holy Grail.', 'davinci.jpg', 17.99, 2003, 'The Da Vinci Code'),
(10, 'A lawyer defends a black man accused of rape.', 'mockingbird.jpg', 13.99, 1960, 'To Kill a Mockingbird');

-- BookAuthor relationships
INSERT INTO BookAuthor (BookId, AuthorId) VALUES
(1, 1),
(2, 2),
(3, 3),
(4, 4),
(5, 5),
(6, 6),
(7, 7),
(8, 8),
(9, 9),
(10, 10);
