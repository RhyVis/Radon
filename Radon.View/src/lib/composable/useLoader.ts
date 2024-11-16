import i18n from "@/locale";
import { useToggle } from "@vueuse/core";
import { MessagePlugin } from "tdesign-vue-next";
import { computed, ref } from "vue";

const { t } = i18n.global;

export type Loader = {
  name: string;
  action: Promise<unknown>;
};

export const useLoader = (loaders: Loader[]) => {
  const current = ref("?");
  const [completed, setCompleted] = useToggle(false);

  const error = ref<string[]>([]);
  const hasError = computed(() => error.value.length > 0);

  const load = async () => {
    for (const loader of loaders) {
      current.value = loader.name;
      try {
        await loader.action;
      } catch (e) {
        console.error(e);
        error.value.push(loader.name);
        await MessagePlugin.warning(t("loader.loadFailed", { name: loader.name }));
      }
    }
    current.value = hasError.value ? `×[${error.value.join("|")}]` : "√";
    setCompleted(true);
  };

  return { load, current, completed, error, hasError };
};
