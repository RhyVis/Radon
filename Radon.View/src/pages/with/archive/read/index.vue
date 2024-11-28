<script setup lang="ts">
import { useNarrow } from "@/composable/useNarrow.ts";
import { getMdRecord } from "@/pages/with/archive/define.ts";
import { useGlobalStore } from "@/store/global.ts";
import { get, set, useDark } from "@vueuse/core";
import { useRouteParams, useRouteQuery } from "@vueuse/router";
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
const other = useRouteQuery("other");

const { localeStandard } = storeToRefs(useGlobalStore());
const name = ref("");
const desc = ref("");
const content = ref("");
const narrow = useNarrow();
const sideWidth = computed(() => (get(narrow) ? "0" : "168px"));
const theme = computed(() => (get(dark) ? "dark" : "light"));
const scrollEl = document.getElementById("base-content") ?? document.documentElement;

const handleBack = () => {
  if (other.value) {
    history.back();
  } else {
    router.push("/archive");
  }
};
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
    <div class="mt-6" v-if="content.length === 0">
      <t-empty :title="t('common.loading')" />
    </div>
    <div v-else>
      <t-layout class="r-md-container">
        <MdPreview
          class="r-md-preview"
          id="preview-only"
          :model-value="content"
          :theme="theme"
          :language="localeStandard"
          previewTheme="cyanosis"
          codeTheme="github"
        />
        <t-aside :width="sideWidth">
          <t-affix :offset-top="160" :offset-bottom="60" container="#base-content">
            <MdCatalog v-if="!narrow" editorId="preview-only" :theme="theme" :scrollElement="scrollEl" />
          </t-affix>
        </t-aside>
      </t-layout>
    </div>

    <template #actions>
      <t-button class="r-no-select" variant="text" theme="primary" shape="circle" @click="handleBack">
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
