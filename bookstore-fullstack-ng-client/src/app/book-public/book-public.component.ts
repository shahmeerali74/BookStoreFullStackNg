import { ChangeDetectionStrategy, Component } from "@angular/core";

@Component({
  selector: "app-book-public",
  standalone: true,
  imports: [],
  template: ` books `,
  styles: [``],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class BookPublicComponent {}
