import vueTsEslintConfig from "@vue/eslint-config-typescript";
import pluginVue from "eslint-plugin-vue";
import pluginVitest from "@vitest/eslint-plugin";
import skipFormatting from "@vue/eslint-config-prettier/skip-formatting";

export default [
  {
    name: "app/files-to-lint",
    files: ["**/*.{ts,mts,tsx,vue}"],
  },

  {
    name: "app/files-to-ignore",
    ignores: ["**/dist/**", "**/dist-ssr/**", "**/coverage/**", "packVersion.cjs"],
  },

  ...pluginVue.configs["flat/essential"],
  ...vueTsEslintConfig(),
  {
    name: "app/various-lang",
    languageOptions: {
      parserOptions: {
        ecmaFeatures: {
          jsx: true,
        },
      },
    },
    rules: {
      "vue/block-lang": [
        "error",
        {
          script: {
            lang: ["ts", "tsx"],
          },
          style: {
            lang: ["less", "sass", "css"],
          },
        },
      ],
      "vue/multi-word-component-names": "off",
    },
  },

  {
    ...pluginVitest.configs.recommended,
    files: ["src/**/__tests__/*"],
  },
  skipFormatting,
];
