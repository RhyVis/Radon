/**
 * @description Refer to ApiResBase defined in core
 */
type ApiResponse<T> = {
  code: number;
  msg: string;
  data: T;
};

/**
 * @description Refer to ExceptionRes defined in core, data is JSON serialized exception object
 */
type ErrResponse = ApiResponse<string>;

export type { ApiResponse, ErrResponse };
