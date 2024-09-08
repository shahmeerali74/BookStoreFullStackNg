import { Routes } from "@angular/router";
import { HomeComponent } from "./home/home.component";

export const routes: Routes = [
  {
    path: "home",
    component: HomeComponent,
  },
  {
    path: "books",
    loadComponent: () =>
      import("./book/book.component").then((m) => m.BookComponent),
  },
  {
    path: "genres",
    loadComponent: () =>
      import("./genre/genre.component").then((c) => c.GenreComponent),
  },
  {
    path: "",
    redirectTo: "/home",
    pathMatch: "full",
  },
  {
    path: "**",
    loadComponent: () =>
      import("./not-found.component").then((c) => c.NotFoundComponent),
  },
];
