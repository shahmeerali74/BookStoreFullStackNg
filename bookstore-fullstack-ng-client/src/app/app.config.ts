import { ApplicationConfig } from "@angular/core";
import { provideRouter } from "@angular/router";

import { routes } from "./app.routes";
import { provideAnimationsAsync } from "@angular/platform-browser/animations/async";
import { provideStore } from "@ngrx/store";
import { provideHttpClient } from "@angular/common/http";
import { provideEffects } from "@ngrx/effects";
import { genreFeatureKey, genreReducers } from "./genre/state/genre.reducers";
import { GenreEffects } from "./genre/state/genre.effects";

const reducers = {
  [genreFeatureKey]: genreReducers,
};

const effects = [GenreEffects];

export const appConfig: ApplicationConfig = {
  providers: [
    provideRouter(routes),
    provideAnimationsAsync(),
    provideStore(reducers),
    provideHttpClient(),
    provideEffects(effects),
  ],
};
