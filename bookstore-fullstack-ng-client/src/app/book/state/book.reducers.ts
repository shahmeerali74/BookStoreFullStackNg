import { HttpErrorResponse } from "@angular/common/http";
import { BookReadModel } from "../Data/book-read.model";
import { createReducer, on } from "@ngrx/store";
import { BookActions } from "./book.actions";

export const bookFeatureKey = "book";

export interface BookState {
  books: ReadonlyArray<BookReadModel>;
  totalCount: number;
  pageNumber: number;
  pageSize: number;
  totalPages: number;
  hasPrevious: boolean;
  hasNext: boolean;
  searchTerm: string | null;
  sortBy: string;
  publishedFrom: number;
  publishedTo: number;
  loading: boolean;
  error: HttpErrorResponse | null;
}

export const initialState: BookState = {
  books: [],
  totalCount: 0,
  pageNumber: 1,
  pageSize: 10,
  totalPages: 0,
  hasPrevious: false,
  hasNext: false,
  searchTerm: "",
  sortBy: "",
  publishedFrom: 0,
  publishedTo: 0,
  loading: false,
  error: null,
};

export const bookReducer = createReducer(
  initialState,
  on(BookActions.loadBooks, (state) => ({ ...state, loading: true })),
  on(BookActions.loadBooksSuccess, (state, { books }) => ({
    ...state,
    books,
    loading: false,
  })),
  on(BookActions.loadBookFailure, (state, { error }) => ({
    ...state,
    error,
    loading: false,
  })),
  on(BookActions.addBook, (state, { book }) => ({
    ...state,
    loading: false,
  })),
  on(BookActions.addBookSucces, (state, { book }) => ({
    ...state,
    books: { ...state.books, book },
    loading: false,
  })),
  on(BookActions.addBookFailure, (state, { error }) => ({
    ...state,
    error,
    loading: false,
  })),
  on(BookActions.updateBook, (state, { book }) => ({
    ...state,
    loading: false,
  })),
  on(BookActions.addBookSucces, (state, { book }) => ({
    ...state,
    books: state.books.filter((b) => (b.id == book.id ? book : b)),
    loading: false,
  })),
  on(BookActions.addBookFailure, (state, { error }) => ({
    ...state,
    error,
    loading: false,
  }))
);
