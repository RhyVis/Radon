<script lang="ts" setup>
import { useKeyUpdate } from "@/composable/useKeyUpdate.ts";
import { useNarrow } from "@/composable/useNarrow.ts";
import ArchiveBackPage from "@/pages/with/archive/comps/ArchiveBackPage.vue";
import { getMdRecord } from "@/pages/with/archive/scripts/function.ts";
import { useGlobalStore } from "@/store/global.ts";
import { get, set, useDark, useTitle, useToggle } from "@vueuse/core";
import { useRouteParams } from "@vueuse/router";
import { MdCatalog, MdPreview } from "md-editor-v3";
import "md-editor-v3/lib/preview.css";
import { storeToRefs } from "pinia";
import { computed, ref, watch } from "vue";
import { useI18n } from "vue-i18n";
import { useRouter } from "vue-router";

const { t } = useI18n();
const { push } = useRouter();
const dark = useDark();
const path = useRouteParams("p");

const { localeStandard } = storeToRefs(useGlobalStore());
const { key, updateKey } = useKeyUpdate();
const [loading, setLoading] = useToggle(false);
const name = ref("");
const desc = ref("");
const content = ref("");
const narrow = useNarrow();
const sideWidth = computed(() => (get(narrow) ? "0" : "168px"));
const theme = computed(() => (get(dark) ? "dark" : "light"));
const noContent = computed(() => content.value.length === 0);
const scrollEl = document.getElementById("base-content") ?? document.documentElement;

const updateContent = async (path: string) => {
  setLoading(true);
  try {
    const record = await getMdRecord(path);
    set(name, record.name);
    set(desc, record.desc);
    set(content, record.content);
    useTitle(record.name);
  } catch (e) {
    console.error(e);
    await push("/archive");
  } finally {
    setLoading(false);
  }
};

watch(
  () => narrow.value,
  () => {
    updateKey();
  },
  { immediate: true },
);
watch(
  () => path.value,
  async newPath => {
    if (newPath) {
      if (typeof newPath === "string") {
        await updateContent(newPath);
      } else {
        await updateContent(newPath[0]);
      }
    }
  },
  { immediate: true },
);
</script>

<template>
  <content-layout id="md-read-container" :subtitle="desc" :title="name">
    <div class="mt-6" v-if="loading">
      <t-empty :title="t('common.loading')" />
    </div>
    <div class="mt-6" v-else-if="noContent">
      <t-empty :title="t('noContent')" />
    </div>
    <div v-else>
      <t-layout class="r-md-container">
        <MdPreview
          class="r-md-preview"
          id="preview-only"
          :language="localeStandard"
          :model-value="content"
          :theme="theme"
          codeTheme="github"
          previewTheme="cyanosis"
        />
        <t-aside :key="key" :width="sideWidth">
          <t-affix :offset-bottom="60" :offset-top="160" container="#base-content">
            <MdCatalog v-if="!narrow" :scrollElement="scrollEl" :theme="theme" editorId="preview-only" />
          </t-affix>
        </t-aside>
      </t-layout>
    </div>

    <template #actions>
      <ArchiveBackPage />
    </template>
  </content-layout>
</template>

<style scoped>
.r-md-container {
  background: var(--td-bg-color-container);

  .r-md-preview {
    border-radius: 16px;
  }
}
</style>

<i18n locale="en">
noContent: No content
</i18n>

<i18n locale="zh-CN">
noContent: 无内容
</i18n>
