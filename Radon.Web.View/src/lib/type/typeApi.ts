/**
 * @description refer to ApiResBase defined in core
 */
type ApiResponse = {
  code: number;
  data: unknown;
  message: string;
};

type CompileTime = {
  compileTime: number;
};

export type { ApiResponse, CompileTime };
