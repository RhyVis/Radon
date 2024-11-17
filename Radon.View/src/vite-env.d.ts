/// <reference types="vite/client" />
/// <reference types="vite-plugin-pwa/client" />

interface ImportMetaEnv {
  readonly VITE_API_ROOT: string;
  readonly VITE_RES_ROOT: string;
}

interface ImportMeta {
  readonly env: ImportMetaEnv;
}
