import { Routes } from "@angular/router";
import { HomeComponent } from "./home/home.component";
import { roleGuard } from "./helpers/role.guard";
import { routeGuard } from "./helpers/route.guard";

export const routes: Routes = [
  {
    path: "home",
    component: HomeComponent,
  },
  {
    path: "manage-books",
    canActivate: [roleGuard],
    data: { roles: ["Admin"] },
    loadComponent: () =>
      import("./book/book.component").then((m) => m.BookComponent),
  },
  {
    path: "genres",
    canActivate: [roleGuard],
    data: { roles: ["Admin"] },
    loadComponent: () =>
      import("./genre/genre.component").then((c) => c.GenreComponent),
  },
  {
    path: "authors",
    canActivate: [roleGuard],
    data: { roles: ["Admin"] },
    loadComponent: () =>
      import("./authors/author.component").then((c) => c.AuthorComponent),
  },
  {
    path: "signup",
    loadComponent: () =>
      import("./account/registration.component").then(
        (c) => c.RegistrationComponent
      ),
  },
  {
    path: "login",
    loadComponent: () =>
      import("./account/login.component").then((c) => c.LoginComponent),
  },
  {
    path: "dashboard",
    canActivate: [routeGuard],
    loadComponent: () =>
      import("./dashboard.component").then((c) => c.DashboardComponent),
  },
  {
    path: "unauthorized",
    loadComponent: () =>
      import("./unauthorized.component").then((c) => c.UnauthorizedComponent),
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
