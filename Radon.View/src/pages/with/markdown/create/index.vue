<script setup lang="tsx">
import { createMdRecord } from "@/pages/with/markdown/define.ts";
import { useGlobalStore } from "@/store/global.ts";
import { get, useDark, useToggle } from "@vueuse/core";
import { useRouteQuery } from "@vueuse/router";
import { MdEditor } from "md-editor-v3";
import "md-editor-v3/lib/style.css";
import { storeToRefs } from "pinia";
import { ArrowLeftIcon } from "tdesign-icons-vue-next";
import { MessagePlugin } from "tdesign-vue-next";
import NotificationPlugin from "tdesign-vue-next/es/notification/plugin";
import { computed, onMounted, ref } from "vue";
import { useI18n } from "vue-i18n";
import { onBeforeRouteLeave, useRouter } from "vue-router";

const dark = useDark();
const router = useRouter();
const createName = useRouteQuery("name");
const createDesc = useRouteQuery("desc");
const content = ref("");
const [complete, setComplete] = useToggle(false);
const { locale } = storeToRefs(useGlobalStore());
const { t } = useI18n();
const theme = computed(() => (get(dark) ? "dark" : "light"));
const lang = computed(() => (get(locale) === "zh-CN" ? "zh-CN" : "en-US"));

const handleCreate = async () => {
  const path = await createMdRecord(get(createName) as string, get(createDesc) as string, get(content));
  if (path.length > 0) {
    setComplete(true);
    void MessagePlugin.success("Create success, moving to write page");
    await router.push(`/md/${path}/write`);
  } else {
    void MessagePlugin.error("Create failed");
  }
};

onBeforeRouteLeave(async () => {
  if (get(complete)) {
    return true;
  } else {
    return await new Promise<boolean>(async resolve => {
      const n = await NotificationPlugin.warning({
        title: t("message.saveConfirm.title"),
        content: t("message.saveConfirm.content"),
        duration: 5000,
        closeBtn: false,
        footer: () => (
          <div class="r-note-box">
            <t-space class="r-note-box-inner" direction="horizontal" size={6}>
              <t-button
                theme="default"
                size="small"
                onClick={() => {
                  resolve(false);
                  n.close();
                }}
              >
                {t("message.saveConfirm.cancel")}
              </t-button>
              <t-button
                theme="warning"
                size="small"
                onClick={() => {
                  resolve(true);
                  n.close();
                }}
              >
                {t("message.saveConfirm.confirm")}
              </t-button>
            </t-space>
          </div>
        ),
        onDurationEnd: () => {
          resolve(false);
        },
      });
    });
  }
});
onMounted(() => {
  if (
    createName.value == null ||
    createDesc.value == null ||
    typeof createName.value !== "string" ||
    typeof createDesc.value !== "string"
  ) {
    MessagePlugin.warning("Invalid create request");
    setComplete(true);
    router.push("/md");
  }
});
</script>

<template>
  <content-layout title="Create">
    <MdEditor
      v-model="content"
      @onSave="handleCreate()"
      :theme="theme"
      :language="lang"
      previewTheme="github"
      codeTheme="github"
      noUploadImg
    />

    <template #actions>
      <t-button class="r-no-select" variant="text" theme="primary" shape="circle" @click="router.push('/md')">
        <ArrowLeftIcon />
      </t-button>
    </template>
  </content-layout>
</template>

<style>
.r-note-box {
  width: 100%;
  text-align: right;
  .r-note-box-inner {
    width: auto;
    margin-top: 4px;
  }
}
</style>
