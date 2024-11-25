<script setup lang="tsx">
import { checkMdRecord, updateMdRecord } from "@/pages/with/markdown/define.ts";
import { useGlobalStore } from "@/store/global.ts";
import { get, set, useDark, useToggle, useWindowSize } from "@vueuse/core";
import { useRouteParams } from "@vueuse/router";
import { MdEditor } from "md-editor-v3";
import "md-editor-v3/lib/style.css";
import { storeToRefs } from "pinia";
import { ArrowLeftIcon } from "tdesign-icons-vue-next";
import { MessagePlugin } from "tdesign-vue-next";
import NotificationPlugin from "tdesign-vue-next/es/notification/plugin";
import { computed, ref, watch } from "vue";
import { useI18n } from "vue-i18n";
import { onBeforeRouteLeave, useRouter } from "vue-router";

const { t } = useI18n();
const router = useRouter();
const dark = useDark();
const path = useRouteParams("p");
const name = ref("");
const desc = ref("");
const content = ref("");
const { locale } = storeToRefs(useGlobalStore());
const { width } = useWindowSize();
const [updating, setUpdating] = useToggle(false);
const [changed, setChanged] = useToggle(false);
const theme = computed(() => (get(dark) ? "dark" : "light"));
const lang = computed(() => (get(locale) === "zh-CN" ? "zh-CN" : "en-US"));
const tooNarrow = computed(() => get(width) < 768);

const updateContent = async (path: string) => {
  const check = await checkMdRecord(path);
  if (check.name.length === 0) {
    void MessagePlugin.error(t("msg.noSuchRecord"));
    await router.push("/md");
  } else {
    set(name, check.name);
    set(desc, check.desc);
    set(content, check.content);
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
  <content-layout :title="name" :subtitle="desc">
    <div>
      <t-empty class="tw-mt-6" v-if="content.length === 0" :title="t('common.loading')" />
      <MdEditor
        v-else
        v-model="content"
        @onChange="setChanged(true)"
        @onSave="handleUpdate()"
        :readOnly="updating"
        :theme="theme"
        :language="lang"
        :preview="!tooNarrow"
        previewTheme="cyanosis"
        codeTheme="github"
        noUploadImg
      />
    </div>

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

<i18n locale="en">
msg:
  updateSuccess: Update success
  updateFailed: Update failed
  noSuchRecord: No such record
  updateInProgress: Update in progress
</i18n>

<i18n locale="zh-CN">
msg:
  updateSuccess: 更新成功
  updateFailed: 更新失败
  noSuchRecord: 无此记录
  updateInProgress: 更新进行中
</i18n>
