import { inject, Injectable } from "@angular/core";
import { environment } from "../../../environments/environment.development";
import { RegistrationModel } from "./registration.model";
import { HttpClient } from "@angular/common/http";

@Injectable({ providedIn: "root" })
export class AccountService {
  private readonly baseUrl = environment.baseUrl + "/api/account";
  private readonly http = inject(HttpClient);

  /**
   * Registers a new user.
   *
   * @param data The user registration data.
   *
   * @returns The registered user.
   */
  registration(data: RegistrationModel) {
    return this.http.post<RegistrationModel>(this.baseUrl + "/signup", data);
  }
}
