<script setup lang="ts">
import AdvancedOptContainer from "@/components/menu/AdvOptContainer.vue";
import ContentLayout from "@/layout/frame/ContentLayout.vue";
import { radixVal } from "@/pages/math/radix/scripts/radix";
import { ArrowRightCircleIcon, RefreshIcon } from "tdesign-icons-vue-next";
import { MessagePlugin } from "tdesign-vue-next";

const defaultCharset = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
const query = reactive({
  input: "10",
  iRadix: 10,
  oRadix: 10,
  iCharset: "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz",
  oCharset: "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz",
});
const iCharsetInputStatus = ref<"default" | "error">("default");
const oCharsetInputStatus = ref<"default" | "error">("default");
const result = computed(() => {
  return radixVal(query.input, query.iRadix, query.oRadix, query.iCharset, query.oCharset);
});

const handleCharsetChange = () => {
  iCharsetInputStatus.value = "default";
  oCharsetInputStatus.value = "default";
  const iL = query.iCharset.length;
  const oL = query.oCharset.length;
  if (query.iRadix > iL && iL > 2) {
    query.iRadix = iL;
  }
  if (query.oRadix > oL && oL > 2) {
    query.oRadix = oL;
  }
  if (iL < 2) {
    MessagePlugin.warning("字符集长度不能小于2");
    iCharsetInputStatus.value = "error";
  }
  if (oL < 2) {
    MessagePlugin.warning("字符集长度不能小于2");
    oCharsetInputStatus.value = "error";
  }
  if (new Set(query.iCharset).size < iL) {
    MessagePlugin.warning("字符集不允许重复字符");
    iCharsetInputStatus.value = "error";
  }
  if (new Set(query.oCharset).size < oL) {
    MessagePlugin.warning("字符集不允许重复字符");
    oCharsetInputStatus.value = "error";
  }
};
const handleCharsetInputReset = () => {
  query.iCharset = defaultCharset;
  query.iRadix = 10;
};
const handleCharsetOutputReset = () => {
  query.oCharset = defaultCharset;
  query.oRadix = 10;
};
</script>

<template>
  <ContentLayout title="进制转换" subtitle="注意输入内容符合字符集">
    <t-form label-align="top">
      <t-form-item label="数字">
        <t-space align="center">
          <t-input class="r-input-text-align-for-num" v-model="query.input" />
          <ArrowRightCircleIcon />
          <t-input class="r-input-text-align-for-num" :value="result" placeholder="输入值来转换" :readonly="true" />
        </t-space>
      </t-form-item>
      <t-form-item label="进制">
        <t-space align="center">
          <t-input-number v-model="query.iRadix" :min="2" :max="query.iCharset.length" />
          <ArrowRightCircleIcon />
          <t-input-number v-model="query.oRadix" :min="2" :max="query.oCharset.length" />
        </t-space>
      </t-form-item>
      <t-form-item label="高级">
        <AdvancedOptContainer>
          <t-space :size="4" direction="vertical" align="center">
            <t-space :size="4">
              <t-tooltip placement="top" content="按顺序定义基数字符，不允许重复">
                <t-tag class="r-no-select" content="输入字符集" />
              </t-tooltip>
              <t-button size="small" theme="default" @click="handleCharsetInputReset">
                <RefreshIcon />
              </t-button>
            </t-space>
            <t-textarea
              v-model="query.iCharset"
              :status="iCharsetInputStatus"
              :autosize="true"
              @change="handleCharsetChange"
            />
            <t-space :size="4">
              <t-tooltip placement="top" content="按顺序定义基数字符，不允许重复">
                <t-tag class="r-no-select" content="输出字符集" />
              </t-tooltip>
              <t-button size="small" theme="default" @click="handleCharsetOutputReset">
                <RefreshIcon />
              </t-button>
            </t-space>
            <t-textarea
              v-model="query.oCharset"
              :status="oCharsetInputStatus"
              :autosize="true"
              @change="handleCharsetChange"
            />
          </t-space>
        </AdvancedOptContainer>
      </t-form-item>
    </t-form>
  </ContentLayout>
</template>

<style scoped>
.r-input-text-align-for-num {
  width: 144px;
}
</style>
