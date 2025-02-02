<script lang="tsx" setup>
import { createMdRecord } from "@/pages/with/archive/scripts/function";
import { useGlobalStore } from "@/store/global.ts";
import { get, useDark, useTitle, useToggle } from "@vueuse/core";
import { useRouteQuery } from "@vueuse/router";
import { MdEditor } from "md-editor-v3";
import "md-editor-v3/lib/style.css";
import { storeToRefs } from "pinia";
import { MessagePlugin } from "tdesign-vue-next";
import NotificationPlugin from "tdesign-vue-next/es/notification/plugin";
import { computed, onMounted, ref } from "vue";
import { useI18n } from "vue-i18n";
import { onBeforeRouteLeave, useRouter } from "vue-router";
import { useNarrow } from "@/composable/useNarrow.ts";
import ArchiveBackPage from "@/pages/with/archive/comps/ArchiveBackPage.vue";
import type { MdUploadImageCallback } from "@/pages/with/archive/scripts/define.ts";
import { apiUploadImageList } from "@/lib/common/apiUploadImage.ts";

const dark = useDark();
const createName = useRouteQuery("name");
const createDesc = useRouteQuery("desc");
const content = ref("");
const [updating, setUpdating] = useToggle(false);
const [complete, setComplete] = useToggle(false);
const { localeStandard } = storeToRefs(useGlobalStore());
const { push } = useRouter();
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
    await push(`/archive/${path}/edit`);
  } else {
    void MessagePlugin.error(t("msg.failed"));
  }
};
const handleUploadImage: MdUploadImageCallback = async (files, callback) => {
  setUpdating(true);
  try {
    const actionResult = await apiUploadImageList(files);
    callback(actionResult.map(item => item.url));
  } catch (e) {
    void MessagePlugin.error(t("msg.imageUploadFailed"));
    console.error(e);
    callback([]);
  } finally {
    setUpdating(false);
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
    push("/archive");
  } else {
    useTitle(get(createName) as string);
  }
});
</script>

<template>
  <content-layout title="Create">
    <MdEditor
      v-model="content"
      :language="localeStandard"
      :preview="!tooNarrow"
      :readOnly="updating"
      :theme="theme"
      codeTheme="github"
      previewTheme="cyanosis"
      @onSave="handleCreate()"
      @onUploadImg="handleUploadImage"
    />

    <template #actions>
      <ArchiveBackPage />
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
