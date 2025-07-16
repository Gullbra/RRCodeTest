export interface Book {
  id: string;
  title: string;
  author: string;
  DateOfPublication: Date;
  userId: string;
}

export interface CreateBookRequest {
  title: string;
  author: string;
  DateOfPublication: Date;
  userId: string;
}