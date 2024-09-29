export interface BookCreateModel {
  id: number;
  title: string;
  description: string;
  price: number;
  publishedYear: number;
  authorIds: number[];
  genreIds: number[];
  ImageFile: File | null;
  ImageUrl: string | null;
}
