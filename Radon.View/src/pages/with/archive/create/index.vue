<script setup lang="tsx">
import { createMdRecord } from "@/pages/with/archive/define.ts";
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
import { useNarrow } from "@/composable/useNarrow.ts";

const dark = useDark();
const router = useRouter();
const createName = useRouteQuery("name");
const createDesc = useRouteQuery("desc");
const content = ref("");
const [updating, setUpdating] = useToggle(false);
const [complete, setComplete] = useToggle(false);
const { localeStandard } = storeToRefs(useGlobalStore());
const { t } = useI18n();
const theme = computed(() => (get(dark) ? "dark" : "light"));
const tooNarrow = useNarrow();

const handleCreate = async () => {
  if (get(updating)) {
    return;
  }
  setUpdating(true);
  const path = await createMdRecord(get(createName) as string, get(createDesc) as string, get(content));
  if (path.length > 0) {
    setComplete(true);
    void MessagePlugin.success(t("msg.success"));
    await router.push(`/archive/${path}/edit`);
  } else {
    void MessagePlugin.error(t("msg.failed"));
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
    MessagePlugin.warning(t("msg.invalidCreate"));
    setComplete(true);
    router.push("/archive");
  }
});
</script>

<template>
  <content-layout title="Create">
    <MdEditor
      v-model="content"
      @onSave="handleCreate()"
      :readOnly="updating"
      :theme="theme"
      :language="localeStandard"
      :preview="!tooNarrow"
      previewTheme="cyanosis"
      codeTheme="github"
      noUploadImg
    />

    <template #actions>
      <t-button class="r-no-select" variant="text" theme="primary" shape="circle" @click="router.push('/archive')">
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

<i18n locale="en">
msg:
  invalidCreate: "Invalid create request"
  success: "Create success, moving to write page"
  failed: "Create failed"
</i18n>

<i18n locale="zh-CN">
msg:
  invalidCreate: "无效的创建请求"
  success: "创建成功，跳转到写作页面"
  failed: "创建失败"
</i18n>
