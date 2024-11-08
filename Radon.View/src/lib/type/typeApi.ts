/**
 * @description refer to ApiResBase defined in core
 */
type ApiResponse<T> = {
  code: number;
  msg: string;
  data: T;
};

type ErrResponse = ApiResponse<string>;

export type { ApiResponse, ErrResponse };
