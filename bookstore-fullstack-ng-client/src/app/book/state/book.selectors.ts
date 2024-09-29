import { createFeatureSelector, createSelector } from "@ngrx/store";
import { bookFeatureKey, BookState } from "./book.reducers";

export const selectBookState = createFeatureSelector<BookState>(bookFeatureKey);

export const selectAuthors = createSelector(
  selectBookState,
  (state) => state.books
);

export const selectAuthorLoading = createSelector(
  selectBookState,
  (state) => state.loading
);

export const selectAuthorError = createSelector(
  selectBookState,
  (state) => state.error
);

export const selectAuthorTotalCount = createSelector(
  selectBookState,
  (state) => state.totalCount
);

export const selectAuthorCurrentPage = createSelector(
  selectBookState,
  (state) => state.pageNumber
);

export const selectAuthorPageSize = createSelector(
  selectBookState,
  (state) => state.pageSize
);

export const selectAuthorTotalPages = createSelector(
  selectBookState,
  (state) => state.totalPages
);

export const selectAuthorHasNext = createSelector(
  selectBookState,
  (state) => state.hasNext
);

export const selectAuthorHasPrevious = createSelector(
  selectBookState,
  (state) => state.hasPrevious
);
