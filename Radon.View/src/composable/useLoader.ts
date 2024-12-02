import i18n from "@/locale";
import { get, set, useOnline, useToggle } from "@vueuse/core";
import { MessagePlugin } from "tdesign-vue-next";
import { computed, ref } from "vue";

const { t } = i18n.global;

export type Loader = {
  name: string;
  action: Promise<void>;
};

export const useLoader = (loaders: Loader[], loaderName: string = "Loader") => {
  const current = ref("?");
  const [initiated, setInitiated] = useToggle(false);
  const [completed, setCompleted] = useToggle(false);

  const error = ref<string[]>([]);
  const hasError = computed(() => error.value.length > 0);

  const online = useOnline();

  const load = async () => {
    setInitiated(true);
    if (!get(online)) {
      await MessagePlugin.warning(t("loader.notOnline", { name: loaderName }));
      error.value.push("OFFLINE");
      set(current, "× OFFLINE");
      setCompleted(true);
      return;
    }
    for (const loader of loaders) {
      set(current, loader.name);
      try {
        await loader.action;
      } catch (e) {
        console.error(e);
        error.value.push(loader.name);
        await MessagePlugin.warning(t("loader.loadFailed", { name: loader.name }));
      }
    }
    set(current, get(hasError) ? `×[${get(error).join("|")}]` : "√");
    setCompleted(true);
  };

  return { load, current, initiated, completed, error, hasError };
};
