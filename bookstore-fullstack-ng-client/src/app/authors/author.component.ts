import { AsyncPipe, NgIf } from "@angular/common";
import { ChangeDetectionStrategy, Component, inject } from "@angular/core";
import { AuthorService } from "./data/author.service";
import { catchError, Observable, of, shareReplay, tap } from "rxjs";
import { PagedList } from "../common/paged-list.model";
import { Author } from "./data/author.model";

@Component({
  selector: "app-author",
  standalone: true,
  imports: [NgIf, AsyncPipe],
  template: ``,
  styles: [``],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class AuthorComponent {}
