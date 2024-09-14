import { AsyncPipe, JsonPipe, NgIf } from "@angular/common";
import { ChangeDetectionStrategy, Component, inject } from "@angular/core";
import { AuthorService } from "./data/author.service";

@Component({
  selector: "app-author",
  standalone: true,
  imports: [NgIf, AsyncPipe, JsonPipe],
  template: `
    <ng-container *ngIf="authors$ as authors">
      {{ authors | json }}
    </ng-container>
  `,
  styles: [``],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class AuthorComponent {
  service = inject(AuthorService);
  authors$ = this.service.getAuthors();
}
