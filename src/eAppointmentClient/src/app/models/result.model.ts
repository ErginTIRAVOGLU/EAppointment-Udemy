export interface Result<T> {
  data?: T | null;
  errorMessage?: string[];
  isSuccess: boolean;
  statusCode: number;
}

export const initialResultModel: Result<any> = {
  data: null,
  errorMessage: [],
  isSuccess: false,
  statusCode: 0,
};
