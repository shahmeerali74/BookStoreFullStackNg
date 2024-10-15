import { createFeatureSelector, createSelector } from "@ngrx/store";
import { userOrderFeaturKey, UserOrderState } from "./user-order.reducers";

export const selectUserOrderState =
  createFeatureSelector<UserOrderState>(userOrderFeaturKey);

export const selectUserOrders = createSelector(
  selectUserOrderState,
  (state) => state.orders
);

export const selectAuthorLoading = createSelector(
  selectUserOrderState,
  (state) => state.loading
);

export const selectAuthorError = createSelector(
  selectUserOrderState,
  (state) => state.error
);

export const selectAuthorTotalCount = createSelector(
  selectUserOrderState,
  (state) => state.totalCount
);

export const selectAuthorCurrentPage = createSelector(
  selectUserOrderState,
  (state) => state.pageNumber
);

export const selectAuthorPageSize = createSelector(
  selectUserOrderState,
  (state) => state.pageSize
);

export const selectAuthorTotalPages = createSelector(
  selectUserOrderState,
  (state) => state.totalPages
);

export const selectAuthorHasNext = createSelector(
  selectUserOrderState,
  (state) => state.hasNext
);

export const selectAuthorHasPrevious = createSelector(
  selectUserOrderState,
  (state) => state.hasPrevious
);
