<script lang="tsx" setup>
import { checkMdRecord, updateMdRecord } from "@/pages/with/archive/scripts/function";
import { useGlobalStore } from "@/store/global.ts";
import { get, set, useDark, useTitle, useToggle } from "@vueuse/core";
import { useRouteParams } from "@vueuse/router";
import { MdEditor } from "md-editor-v3";
import "md-editor-v3/lib/style.css";
import { storeToRefs } from "pinia";
import { MessagePlugin } from "tdesign-vue-next";
import NotificationPlugin from "tdesign-vue-next/es/notification/plugin";
import { computed, ref, watch } from "vue";
import { useI18n } from "vue-i18n";
import { onBeforeRouteLeave, useRouter } from "vue-router";
import { useNarrow } from "@/composable/useNarrow.ts";
import ArchiveBackPage from "@/pages/with/archive/comps/ArchiveBackPage.vue";
import { apiUploadImageList } from "@/lib/common/apiUploadImage.ts";
import type { MdUploadImageCallback } from "@/pages/with/archive/scripts/define.ts";

const { t } = useI18n();
const { push } = useRouter();
const dark = useDark();
const path = useRouteParams("p");
const name = ref("");
const desc = ref("");
const content = ref("");
const { localeStandard } = storeToRefs(useGlobalStore());
const [updating, setUpdating] = useToggle(false);
const [changed, setChanged] = useToggle(false);
const [loading, setLoading] = useToggle(false);
const theme = computed(() => (get(dark) ? "dark" : "light"));
const tooNarrow = useNarrow();

const updateContent = async (path: string) => {
  setLoading(true);
  try {
    const check = await checkMdRecord(path);
    if (check.name.length === 0) {
      void MessagePlugin.error(t("msg.noSuchRecord"));
      await push("/archive");
    } else {
      set(name, check.name);
      set(desc, check.desc);
      set(content, check.content);
      useTitle(check.name);
    }
  } catch {
    void MessagePlugin.error(t("msg.noSuchRecord"));
    await push("/archive");
  } finally {
    setLoading(false);
  }
};
const handleUpdate = () => {
  if (get(updating)) {
    return;
  } else {
    setUpdating(true);
  }
  updateMdRecord(get(path) as string, get(name), get(desc), get(content))
    .then(() => MessagePlugin.success(t("msg.updateSuccess")))
    .catch(() => MessagePlugin.error(t("msg.updateFailed")))
    .finally(() => {
      setUpdating(false);
      setChanged(false);
    });
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
  if (get(updating)) {
    void MessagePlugin.warning(t("msg.updateInProgress"));
    return false;
  } else if (get(changed)) {
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
  } else {
    return true;
  }
});
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
  <content-layout :subtitle="desc" :title="name">
    <div>
      <div class="mt-6" v-if="loading">
        <t-empty :title="t('common.loading')" />
      </div>
      <MdEditor
        v-else
        v-model="content"
        :language="localeStandard"
        :preview="!tooNarrow"
        :readOnly="updating"
        :theme="theme"
        codeTheme="github"
        previewTheme="cyanosis"
        @onChange="setChanged(true)"
        @onSave="handleUpdate()"
        @onUploadImg="handleUploadImage"
      />
    </div>

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
noContent: No content
msg:
  updateSuccess: Update success
  updateFailed: Update failed
  noSuchRecord: No such record
  updateInProgress: Update in progress
  imageUploadFailed: Image upload failed
</i18n>

<i18n locale="zh-CN">
noContent: 无内容
msg:
  updateSuccess: 更新成功
  updateFailed: 更新失败
  noSuchRecord: 无此记录
  updateInProgress: 更新进行中
  imageUploadFailed: 图片上传失败
</i18n>
