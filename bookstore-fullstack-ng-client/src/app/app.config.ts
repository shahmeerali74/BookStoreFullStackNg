import { ApplicationConfig } from "@angular/core";
import { provideRouter } from "@angular/router";

import { routes } from "./app.routes";
import { provideAnimationsAsync } from "@angular/platform-browser/animations/async";
import { provideStore } from "@ngrx/store";
import { provideHttpClient, withInterceptors } from "@angular/common/http";
import { provideEffects } from "@ngrx/effects";
import { genreFeatureKey, genreReducers } from "./genre/state/genre.reducers";
import { GenreEffects } from "./genre/state/genre.effects";
import {
  accountFeatureKey,
  accountReducers,
} from "./account/state/account.reducers";
import { AccountEffects } from "./account/state/account.effects";
import { authInterceptor } from "./helpers/auth.interceptor";

const reducers = {
  [genreFeatureKey]: genreReducers,
  [accountFeatureKey]: accountReducers,
};

const effects = [GenreEffects, AccountEffects];

export const appConfig: ApplicationConfig = {
  providers: [
    provideRouter(routes),
    provideAnimationsAsync(),
    provideStore(reducers),
    provideHttpClient(withInterceptors([authInterceptor])),
    provideEffects(effects),
  ],
};
