import { HttpClient } from "@angular/common/http";
import { inject, Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { PaymentMethodModel } from "./payment-method.model";
import { environment } from "../../../environments/environment.development";
import { OrderCreateModel } from "./order-create.model";

@Injectable({ providedIn: "root" })
export class OrderService {
  http = inject(HttpClient);
  baseUrl = environment.baseUrl + "/api/orders";

  getPaymentMethods = (): Observable<Array<PaymentMethodModel>> =>
    this.http.get<Array<PaymentMethodModel>>(this.baseUrl + "/payment-methods");

  createOrder = (data: OrderCreateModel) =>
    this.http.post<any>(this.baseUrl, data);
}
