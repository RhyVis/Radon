<script setup lang="ts">
import { getMdRecord } from "@/pages/with/markdown/define.ts";
import { useGlobalStore } from "@/store/global.ts";
import { get, set, useDark, useWindowSize } from "@vueuse/core";
import { useRouteParams } from "@vueuse/router";
import { MdCatalog, MdPreview } from "md-editor-v3";
import "md-editor-v3/lib/preview.css";
import { storeToRefs } from "pinia";
import { ArrowLeftIcon } from "tdesign-icons-vue-next";
import { computed, ref, watch } from "vue";
import { useI18n } from "vue-i18n";
import { useRouter } from "vue-router";

const { t } = useI18n();
const router = useRouter();
const dark = useDark();
const path = useRouteParams("p");

const { locale } = storeToRefs(useGlobalStore());
const { width } = useWindowSize();
const name = ref("");
const desc = ref("");
const content = ref("");
const sideWidth = computed(() => (get(width) > 768 ? "168px" : "0"));
const sideShow = computed(() => get(width) > 768);
const theme = computed(() => (get(dark) ? "dark" : "light"));
const lang = computed(() => (get(locale) === "zh-CN" ? "zh-CN" : "en-US"));

const updateContent = async (path: string) => {
  const record = await getMdRecord(path);
  set(name, record.name);
  set(desc, record.desc);
  set(content, record.content);
};

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
  <content-layout id="md-read-container" :title="name" :subtitle="desc">
    <t-empty class="tw-mt-6" v-if="content.length === 0" :title="t('common.loading')" />
    <div v-else>
      <t-layout class="r-md-container">
        <MdPreview
          class="r-md-preview"
          id="preview-only"
          :model-value="content"
          :theme="theme"
          :language="lang"
          previewTheme="github"
          codeTheme="github"
        />
        <t-aside :width="sideWidth">
          <MdCatalog class="r-md-catalog" v-if="sideShow" editor-id="preview-only" :theme="theme" />
        </t-aside>
      </t-layout>
    </div>

    <template #actions>
      <t-button class="r-no-select" variant="text" theme="primary" shape="circle" @click="router.push('/md')">
        <ArrowLeftIcon />
      </t-button>
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

.r-md-catalog {
  position: fixed;
  top: 160px;
  right: 5px;
  width: 168px;
}
</style>
