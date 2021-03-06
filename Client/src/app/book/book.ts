export interface Book {
  id: number;
  name: string;
  totalQuantity: number;
  availableQuantity: number;
  description: string;
  author: string;
  coverImage: string;
  publisher: string;
  isbn: number;
  publicationDate: Date;
  pages: string;
  maxRows: number;
}