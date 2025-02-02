﻿<script lang="ts" setup>
import BtnHome from "@/components/btn/BtnHome.vue";
import PpDrawerEvent from "@/pages/with/pdx-parser/comps/PpDrawerEvent.vue";
import PpReplacer from "@/pages/with/pdx-parser/comps/PpReplacer.vue";
import PpStickyTool from "@/pages/with/pdx-parser/comps/PpStickyTool.vue";
import PdxParserUploadDialog from "@/pages/with/pdx-parser/comps/PpUploadDialog.vue";
import { usePdxTextRender } from "@/pages/with/pdx-parser/comps/usePdxTextRender.ts";
import "@/pages/with/pdx-parser/pdx-color.css";
import type { PdxLangEventItem } from "@/pages/with/pdx-parser/scripts/define.ts";
import { sepTextContent } from "@/pages/with/pdx-parser/scripts/function.ts";
import { usePdxStore } from "@/pages/with/pdx-parser/scripts/store.ts";
import { useToggle } from "@vueuse/core";
import { storeToRefs } from "pinia";
import { InfoCircleIcon, Move1Icon, RefreshIcon, UploadIcon } from "tdesign-icons-vue-next";
import { computed, ref } from "vue";
import { useI18n } from "vue-i18n";

const { t } = useI18n();
const { eventSelectId } = storeToRefs(usePdxStore());
const [menuUploadVisible, setMenuUploadVisible] = useToggle(false);
const [menuReplaceVisible, setMenuReplaceVisible] = useToggle(false);
const [stickyToolOnLeft, setStickyToolOnLeft] = useToggle(false);

const eventDrawerVisible = ref(false);
const eventResult = ref<PdxLangEventItem[]>([]);
const eventSel = computed(() => {
  const findNonEmptyName = (id: number): string => {
    if (id < 0) return "";
    const name = eventResult.value[id]?.name || "";
    return name.length > 0 ? name : findNonEmptyName(id - 1);
  };

  const id = eventSelectId.value;
  const event = eventResult.value[id];
  return {
    ...event,
    name: event?.name.length > 0 ? event.name : findNonEmptyName(id - 1),
  };
});
const eventExist = computed(() => eventResult.value.length > 0);

const { textRender, textAlias } = usePdxTextRender();
</script>

<template>
  <content-layout :title="t('title')">
    <div class="mb-20 w-full" v-if="eventExist">
      <t-space class="w-full" direction="vertical">
        <t-card
          class="w-full"
          v-if="eventSel"
          :id="`pdx-event-${eventSelectId}`"
          :key="eventSelectId"
          :header-bordered="true"
          :title="textAlias(eventSel.name)"
        >
          <t-space class="w-full" direction="vertical">
            <div v-for="(line, lineKey) in sepTextContent(eventSel.desc)" v-html="textRender(line)" :key="lineKey" />
            <t-space class="m-auto w-full" direction="vertical">
              <t-card
                class="r-no-select m-auto w-full"
                v-for="(opt, optKey) in eventSel.options"
                :key="optKey"
                :title="opt.showResp ? textAlias(opt.name) : undefined"
                size="small"
                @click="
                  () => {
                    if (opt.resp.length > 0) opt.showResp = !opt.showResp;
                  }
                "
              >
                <t-tooltip :content="opt.tooltip.length > 0 ? opt.tooltip : undefined">
                  <t-space class="text-center" v-if="!opt.showResp" :size="4">
                    <InfoCircleIcon class="-translate-y-px" v-if="opt.resp.length > 0" size="16px" />
                    <t-text>{{ textAlias(opt.name) }}</t-text>
                  </t-space>
                  <div
                    v-for="(respLine, respKey) in sepTextContent(opt.resp)"
                    v-else
                    v-html="textRender(respLine)"
                    :key="respKey"
                  />
                </t-tooltip>
              </t-card>
            </t-space>
          </t-space>
          <template #actions>
            <span class="r-no-select font-mono">{{ eventSelectId }}</span>
          </template>
        </t-card>
      </t-space>
    </div>
    <div class="mt-6" v-else>
      <t-empty :description="t('empty.description')" :title="t('empty.title')" />
    </div>

    <!-- TopBar Actions -->
    <template #actions>
      <t-button shape="circle" theme="primary" variant="text" @click="setMenuReplaceVisible()">
        <RefreshIcon />
      </t-button>
      <t-button shape="circle" theme="primary" variant="text" @click="setStickyToolOnLeft()">
        <Move1Icon />
      </t-button>
      <t-button shape="circle" theme="primary" variant="text" @click="setMenuUploadVisible()">
        <UploadIcon />
      </t-button>
      <BtnHome />
    </template>

    <!-- Sticky Tool -->
    <PpStickyTool
      v-model:drawer-event-visible="eventDrawerVisible"
      v-model:menu-replace-visible="menuReplaceVisible"
      :event-result-len="eventResult.length"
      :on-left="stickyToolOnLeft"
      type="event"
    />

    <!-- Event Drawer -->
    <PpDrawerEvent v-model:visible="eventDrawerVisible" :event-result="eventResult" />

    <!-- Upload Dialog -->
    <PdxParserUploadDialog
      v-model:menu-upload-visible="menuUploadVisible"
      v-model:parse-result-event="eventResult"
      type="event"
    />

    <!-- Replace Dialog -->
    <PpReplacer v-model:visible="menuReplaceVisible" />
  </content-layout>
</template>

<i18n locale="en">
title: PDX Event Parser
empty:
  title: No Content
  description: Click the upload button on the top right to parse the content
</i18n>

<i18n locale="zh-CN">
title: PDX 事件解析器
empty:
  title: 无内容
  description: 点击右上角上传按钮解析内容
</i18n>
