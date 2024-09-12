import { Component, inject, OnDestroy, OnInit } from "@angular/core";
import { Router, RouterOutlet } from "@angular/router";
import { HeaderComponent } from "./header.component";
import { FooterComponent } from "./footer.component";
import { Store } from "@ngrx/store";
import { MatSnackBar } from "@angular/material/snack-bar";
import { catchError, of, Subject, takeUntil, tap } from "rxjs";
import {
  selectLoginState,
  selectTokenState,
  selectUserInfo,
} from "./account/state/account.selectors";
import accountActions from "./account/state/account.actions";
import { tokenUtils } from "./utils/token.utils";
import { AsyncPipe, NgIf } from "@angular/common";

@Component({
  selector: "app-root",
  standalone: true,
  imports: [RouterOutlet, HeaderComponent, FooterComponent, NgIf, AsyncPipe],
  template: `
    <app-header
      (logout)="logout()"
      [cartCount]="0"
      [isLoggedIn]="(isLoggedIn$ | async) ?? false"
      [user]="userInfo$ | async"
    />

    <div class="content">
      <router-outlet />
    </div>
    <app-footer />
  `,
  styles: [
    `
      :host {
        display: flex;
        flex-direction: column;
        min-height: 100vh;
      }
      .content {
        padding: 15px 20px;
      }
    `,
  ],
})
export class AppComponent implements OnInit, OnDestroy {
  store = inject(Store);
  snackBar = inject(MatSnackBar);
  destroyed$ = new Subject<boolean>();
  router = inject(Router);
  cartLoaded = false;

  token$ = this.store.select(selectTokenState);
  isLoggedIn$ = this.store.select(selectLoginState);
  userInfo$ = this.store.select(selectUserInfo);

  logout() {
    this.store.dispatch(accountActions.logout());
    // empty cart state

    // empty cart Item state

    this.snackBar.open("Successfully logged out", "Dismis", {
      duration: 1000,
    });
    this.router.navigate(["/login"]);
  }

  loadAuthInfo() {
    this.token$
      .pipe(
        tap((token) => {
          if (!token) {
            //retrieve token
            const accessToken = tokenUtils.getToken();
            if (accessToken) {
              // set loginResponse state here on application start
              this.store.dispatch(
                accountActions.loadAccountInfo({ accessToken })
              );
              const user = tokenUtils.getUserFromToken(accessToken);
              if (user?.username) {
                //   this.store.dispatch(
                //     CartActions.loadCart({ username: user.username })
                //   );
              }
            }
          }
        }),
        catchError((error) => {
          console.log(error);
          return of(error);
        }),
        takeUntil(this.destroyed$)
      )
      .subscribe();
  }

  ngOnInit(): void {
    //this.loadAuthInfo();
    //this.loadCart();
  }

  ngOnDestroy(): void {
    this.destroyed$.next(true);
    this.destroyed$.unsubscribe();
  }
}
