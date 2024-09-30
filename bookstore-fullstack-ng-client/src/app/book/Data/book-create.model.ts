export interface BookCreateModel {
  id: number;
  title: string;
  description: string;
  price: number;
  publishedYear: number;
  authorIds: number[];
  genreIds: number[];
  imageFile?: File | null;
  imageUrl?: string | null;
}
