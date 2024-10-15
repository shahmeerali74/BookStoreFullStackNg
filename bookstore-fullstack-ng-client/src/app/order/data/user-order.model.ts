import { BookReadModel } from "../../book/Data/book-read.model";

export interface UserOrderModel {
  id: number;
  email: string;
  name: string;
  mobileNumber: string;
  orderDate: Date;
  orderStatus: number;
  payment: null;
  orderItems: OrderItem[];
}

export interface OrderItem {
  id: number;
  orderId: number;
  quantity: number;
  price: number;
  book: BookReadModel;
}
